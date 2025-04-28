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




        public async Task<(List<Maststate> states, Dictionary<string, int> counts, Dictionary<string, int> countsRet)> GetAllStatesAsync()
        {
            var states = await _dbEcheckContext.Maststates
                .Select(s => new Maststate
                {
                    Stateid = s.Stateid,
                    Statedesc = s.Statedesc,
                    Stactive = s.Stactive
                }).ToListAsync();

            var counts = await _dbEcheckContext.Nctempcnts
                .GroupBy(c => c.Cstate)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var countsRet = await _dbEcheckContext.Nctemprets
                .GroupBy(c => c.Rstate)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            return (states, counts, countsRet);
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
                _dbEcheckContext.Nctempcnts.Add(new Nctempcnt
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

        public async Task DeleteTemplateAsync(int id)
        {
            var entity = await _dbEcheckContext.Nctempcnts.FirstOrDefaultAsync(e => e.Cid == id);
            if (entity != null)
            {
                _dbEcheckContext.Nctempcnts.Remove(entity);
                await _dbEcheckContext.SaveChangesAsync();
            }
        }

        //----------------------------------START-------------------------------RETURNS-------------------------------------------------------------//

        public async Task<List<StateTemplateRetViewModel>> GetTemplateRetByStateAsync(string stateId)
        {
            return await _dbEcheckContext.Nctemprets
                .Where(t => t.Rstate == stateId)
                .Select(t => new StateTemplateRetViewModel
                {
                    Rcode = t.Rcode,
                    Rstate = t.Rstate,
                    Rtype = t.Rtype,
                    Rtitle = t.Rtitle,
                    Rform = t.Rform,
                    Rd = t.Rd,
                    Rm = t.Rm ?? 0,
                    Yroff = t.Yroff,
                    Roblig = t.Roblig,
                    Ract = t.Ract,
                    Ractive = t.Ractive,
                })
                .ToListAsync();
        }

        public async Task<StateTemplateRetViewModel> GetTemplateRetByIdAsync(int rcode)
        {
            var entity = await _dbEcheckContext.Nctemprets.FindAsync(rcode);
            if (entity == null) return null;

            return new StateTemplateRetViewModel
            {
               Rcode = entity.Rcode,
               Rstate = entity.Rstate,
               Rtype = entity.Rtype,
               Rtitle = entity.Rtitle,
               Rform = entity.Rform,
               Rd = entity.Rd,
               Rm = entity.Rm ?? 0,
               Yroff = entity.Yroff,
               Roblig = entity.Roblig,
               Ract = entity.Ract,
               Ractive = entity.Ractive,

            };
        }

        public async Task AddOrUpdateTemplateRetAsync(StateTemplateRetViewModel entry)
        {
            var existing = await _dbEcheckContext.Nctemprets.FindAsync(entry.Rcode);

            if (existing != null)
            {
                // Update
                existing.Rtype = entry.Rtype;
                existing.Rtitle = entry.Rtitle;
                existing.Rform  = entry.Rform;
                existing.Rd = entry.Rd;
                existing.Rm = entry.Rm;
                existing.Yroff = entry.Yroff;
                existing.Roblig = entry.Roblig;
                existing.Ract = entry.Ract;
                existing.Ractive = entry.Ractive;
                
            }
            else
            {
                // Add
                _dbEcheckContext.Nctemprets.Add(new Nctempret
                {
                    Rstate = entry.Rstate,
                    Rtype = entry.Rtype,
                    Rtitle = entry.Rtitle,
                    Rform = entry.Rform,
                    Rd = entry.Rd,
                    Rm = entry.Rm,
                    Yroff = entry.Yroff,    
                    Roblig = entry.Roblig,
                    Ract = entry.Ract,
                    Ractive = entry.Ractive
                });
            }

            await _dbEcheckContext.SaveChangesAsync();
        }

        public async Task DeleteTemplateRetAsync(int id)
        {
            var entity = await _dbEcheckContext.Nctemprets.FirstOrDefaultAsync(e => e.Rcode == id);
            if (entity != null)
            {
                _dbEcheckContext.Nctemprets.Remove(entity);
                await _dbEcheckContext.SaveChangesAsync();
            }
        }

        //----------------------------------END-------------------------------RETURNS-----------------------------------------------------------------//




    }
}


