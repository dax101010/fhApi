using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fhApi.Data.Entities;
using Newtonsoft.Json;


namespace fhApi.Data
{
	public class DocumentRepository : IDocumentRepository
	{
		private readonly IMapper _mapper;
		private readonly string _folder;

		public DocumentRepository(IMapper mapper)
		{
			_mapper = mapper;
			_folder = Environment.GetEnvironmentVariable("TEMP");
		}
		public async Task<Document> LoadAsync(string filename)
		{
			string documentWithoutContentSerialized;
			try
			{
				 documentWithoutContentSerialized = await System.IO.File.ReadAllTextAsync(_folder + filename + ".json");
			}
			catch (FileNotFoundException)
			{
				return null;
			}
			DocumentWithoutContent documentWithoutContent = JsonConvert.DeserializeObject<DocumentWithoutContent>(documentWithoutContentSerialized);
			Document document = _mapper.Map<Document>(documentWithoutContent);
			string fileExtension = MimeTypeFileExtension.GetFileExtensionFromMimeType(document.MimeType);
			string contentSerialized = await System.IO.File.ReadAllTextAsync(_folder + filename + fileExtension);
			document.Content = JsonConvert.DeserializeObject<int[]>(contentSerialized);
			return document;
		}

		public async Task<bool> SaveAsync(Document document)
		{
			//The following 2 tasks where chosen for parallel execution as the logical sequence allows it and the 
			//application could get a performance improvement due to the CPU-bound nature of them.
			Task<string> serializeContentTask = Task.Run(() => { return JsonConvert.SerializeObject(document.Content);  });
			Task<DocumentWithoutContent> documentMapperTask = Task.Run(() => { return _mapper.Map<DocumentWithoutContent>(document); });
			await Task.WhenAll(serializeContentTask, documentMapperTask);
			string content = serializeContentTask.Result;
			DocumentWithoutContent documentWithoutContent = documentMapperTask.Result;

			string documentWithoutContentSerialized = JsonConvert.SerializeObject(documentWithoutContent);
			string fileExtension = MimeTypeFileExtension.GetFileExtensionFromMimeType(document.MimeType);

			bool success;
			try
			{
				//The logical sequence of this section allows the following two tasks to run in parallel as well 
				//but due the I/O-bound nature of them it will downgrade the performance of the process instead of improve it.
				await File.WriteAllTextAsync(_folder + documentWithoutContent.FileName + fileExtension, content);
				await File.WriteAllTextAsync(_folder + documentWithoutContent.FileName + ".json", documentWithoutContentSerialized);
				success = true;
			}
			catch (Exception)
			{
				success = false;
			}
			return success;			
		}
	}
}
