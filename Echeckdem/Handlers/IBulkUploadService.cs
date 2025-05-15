using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Echeckdem.Handlers
{
    public interface IBulkUploadService
    {
        Task<int> UploadLocationDataAsync(IFormFile file, string oid );
    }
}
