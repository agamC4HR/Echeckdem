using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Mono.TextTemplating;

namespace Echeckdem.Services
{
    public interface IStateTemplateService
    {
        Task<(List<Maststate> states, Dictionary<string, int> counts)> GetAllStatesAsync();


        Task<List<StateTemplateViewModel>> GetTemplatesByStateAsync(string stateId);
        Task<StateTemplateViewModel> GetTemplateByIdAsync(int cid);
        Task AddOrUpdateTemplateAsync(StateTemplateViewModel entry);

        Task DeleteTemplateAsync(int id);


    }
}
