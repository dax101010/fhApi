using AutoMapper;
using fhApi.Data.Entities;
using fhApi.Models;

namespace fhApi.Data
{
	public class DocumentProfile : Profile
	{
		public DocumentProfile()
		{
			this.CreateMap<Document, DocumentWithoutContent>()
			.ReverseMap();

			this.CreateMap<Document, DocumentModel>()
			.ReverseMap();

		}

	}
}
