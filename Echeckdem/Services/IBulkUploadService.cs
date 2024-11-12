using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Echeckdem.Services
{
    public interface IBulkUploadService
    {
        Task<int> UploadLocationDataAsync(IFormFile file);
    }
}
