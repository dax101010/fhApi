using fhApi.Data.Entities;
using System.Threading.Tasks;

namespace fhApi.Data
{
	public interface IDocumentRepository
	{
		Task<Document> LoadAsync(string filename);
		Task<bool> SaveAsync(Document document);
	}
}
