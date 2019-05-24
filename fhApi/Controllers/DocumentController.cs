using System;
using System.Threading.Tasks;
using AutoMapper;
using fhApi.Data;
using fhApi.Data.Entities;
using fhApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fhApi.Controllers
{

	[Route("api/v0{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class DocumentController : ControllerBase
	{
		private readonly IDocumentRepository _repository;
		private readonly IMapper _mapper;

		public DocumentController(IDocumentRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet("{filename}")]
		public async Task<ActionResult<DocumentModel>> Get(string filename)
		{
			try
			{
				var result = await _repository.LoadAsync(filename);
				if (result == null) return NotFound();
				return _mapper.Map<DocumentModel>(result);
			}
			catch (Exception)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, "File System Failure");
			}
		}

		[HttpPost("{filename}")]
		public async Task<ActionResult<DocumentModel>> Post(string filename, [FromBody] DocumentModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (model.FileName != filename)
			{
				return BadRequest("The filename defined at route does not match with the one from the body");
			}


			Document document = _mapper.Map<Document>(model);
			if (await _repository.SaveAsync(document))
			{
				//MVC Core 2.2 will provide the Linkgenerator class that avoids hard coding the returned URL 
				return Created($"/api/v01/document/{document.FileName}", model);
			}
			else
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, "File System Failure");

			}

		}

	}
}
