using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class ActScopeSetupService
    {
        private readonly DbEcheckContext _context;

        public ActScopeSetupService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<List<BocwScope>> GetAllScopes()                                 // View scopes from BOCWSCOPES
        {
            return await _context.BocwScopes.ToListAsync();
        }


        public async Task AddScope(BocwScope boscope)                                     // Add Scopes or data in BOCWSCOPES
        {
            var scopeId = GenerateScopeId();
            boscope.ScopeId = scopeId;
            

            _context.BocwScopes.Add(boscope);
            await _context.SaveChangesAsync();
        }

    

        private string GenerateScopeId()                                                                                                  // Logic to generate a 3-character scope ID (e.g., "S01", "S02", etc.)
        {                                                            
            var scopeCount = _context.BocwScopes.Count();
            var newId = "S" + (scopeCount + 1).ToString("D2"); 

            return newId;
        }

        public async Task UpdateScope(BocwScope updatedboscope)
        {
            var existingScope = await _context.BocwScopes.FirstOrDefaultAsync(s => s.ScopeId == updatedboscope.ScopeId);

            if (existingScope != null)
            {
                existingScope.ScopeName = updatedboscope.ScopeName;
                existingScope.ScopeActive = updatedboscope.ScopeActive;

                await _context.SaveChangesAsync();
            }
        }
    }
}

