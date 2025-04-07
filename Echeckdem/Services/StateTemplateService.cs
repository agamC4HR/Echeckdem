using DocumentFormat.OpenXml.InkML;
using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace Echeckdem.Services
{
    public class StateTemplateService : IStateTemplateService
    {
        private readonly DbEcheckContext _dbEcheckContext;

        public StateTemplateService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public async Task<List<Maststate>> GetAllStatesAsync()
        {
            return await _dbEcheckContext.Maststates
                .Select(s => new Maststate
                {
                    Stateid = s.Stateid,
                    Statedesc = s.Statedesc,
                    Stactive = s.Stactive
                })
                .ToListAsync();
        }

        public async Task<List<StateTemplateViewModel>> GetTemplatesByStateAsync(string stateId)
        {
            return await _dbEcheckContext.Nctempcnts
                .Where(t => t.Cstate == stateId)
                .Select(t => new StateTemplateViewModel
                {
                    Cid = t.Cid,
                    CState = t.Cstate,
                    Tp = t.Tp,
                    Freq = t.Freq,
                    Period = t.Period,
                    Ld = t.Ld,
                    Moffset = t.Moffset,
                    Active = t.Active
                })
                .ToListAsync();
        }

        public async Task<StateTemplateViewModel> GetTemplateByIdAsync(int cid)
        {
            var entity = await _dbEcheckContext.Nctempcnts.FindAsync(cid);
            if (entity == null) return null;

            return new StateTemplateViewModel
            {
                Cid = entity.Cid,
                CState = entity.Cstate,
                Tp = entity.Tp,
                Freq = entity.Freq,
                Period = entity.Period,
                Ld = entity.Ld,
                Moffset = entity.Moffset,
                Active = entity.Active
            };
        }

        public async Task AddOrUpdateTemplateAsync(StateTemplateViewModel entry)
        {
            var existing = await _dbEcheckContext.Nctempcnts.FindAsync(entry.Cid);

            if (existing != null)
            {
                // Update
                existing.Tp = entry.Tp;
                existing.Freq = entry.Freq;
                existing.Period = entry.Period;
                existing.Ld = entry.Ld;
                existing.Moffset = entry.Moffset;
                existing.Active = entry.Active;
            }
            else
            {
                // Add
                _dbEcheckContext.Nctempcnts.Add(new  Nctempcnt
                {
                    Cstate = entry.CState,
                    Tp = entry.Tp,
                    Freq = entry.Freq,
                    Period = entry.Period,
                    Ld = entry.Ld,
                    Moffset = entry.Moffset,
                    Active = entry.Active
                });
            }

            await _dbEcheckContext.SaveChangesAsync();
        }
    }
}
