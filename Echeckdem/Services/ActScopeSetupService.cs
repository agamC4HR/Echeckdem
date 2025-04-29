using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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


        private string GenerateScopeId()
        {
            var lastScope = _context.BocwScopes
                                   .OrderByDescending(x => x.ScopeId)
                                   .FirstOrDefault();

            if (lastScope != null)
            {
                string lastId = lastScope.ScopeId.Substring(1); // Remove 'S' prefix
                if (int.TryParse(lastId, out int numericPart))
                {
                    return "S" + (numericPart + 1).ToString("D2");
                }
            }

            // If no record exists, start from S01
            return "S01";
        }

       
        public async Task UpdateScope(BocwScope updatedboscope)
        {
            var existingScope = await _context.BocwScopes.FirstOrDefaultAsync(s => s.ScopeId == updatedboscope.ScopeId);

            if (existingScope != null)
            {
                existingScope.ScopeName = updatedboscope.ScopeName;
                existingScope.ScopeActive = updatedboscope.ScopeActive;
                existingScope.Category = updatedboscope.Category;
                existingScope.Frequency = updatedboscope.Frequency;
                

                await _context.SaveChangesAsync();
            }
        }
    }
}

