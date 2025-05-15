using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Mono.TextTemplating;

namespace Echeckdem.Handlers
{
    public interface IStateTemplateService
    {
        Task<(List<Maststate> states, Dictionary<string, int> counts, Dictionary<string, int> countsRet)> GetAllStatesAsync();

        // ------------------------------START---------------------------------CONTRIBUTIONS-------------------------------------------------------------//
        Task<List<StateTemplateViewModel>> GetTemplatesByStateAsync(string stateId);
        Task<StateTemplateViewModel> GetTemplateByIdAsync(int cid);
        Task AddOrUpdateTemplateAsync(StateTemplateViewModel entry);

        Task DeleteTemplateAsync(int id);
        // ------------------------------END---------------------------------CONTRIBUTIONS-------------------------------------------------------------//

        // ------------------------------START------------------------------RETURNS-------------------------------------------------------------//
        Task<List<StateTemplateRetViewModel>> GetTemplateRetByStateAsync(string stateId);
        Task<StateTemplateRetViewModel> GetTemplateRetByIdAsync(int rcode);
        Task AddOrUpdateTemplateRetAsync(StateTemplateRetViewModel entry);

        Task DeleteTemplateRetAsync(int id);

        // ------------------------------END---------------------------------RETURNS-------------------------------------------------------------//
    }
}
