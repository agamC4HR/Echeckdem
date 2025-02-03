using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.Models;
using System.Threading.Tasks;

namespace Echeckdem.Controllers
{
    public class ActScopeSetupController : Controller
    {
        private readonly ActScopeSetupService _actscopesetup;

        public ActScopeSetupController(ActScopeSetupService actscopesetup)
        {
            _actscopesetup = actscopesetup;
        }

        public async Task<IActionResult> Index()                                               // View scope data from BOCWSCOPES
        {
            var scopes = await _actscopesetup.GetAllScopes();
            return View(scopes);  // Pass data to the view
        }


        public IActionResult Create()
        {
            return View(new BocwScope());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]                                                                                                          // Add scope data into BOCWSCOPES
        public async Task<IActionResult> Create([Bind("ScopeName,ScopeActive")] BocwScope boscope)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _actscopesetup.AddScope(boscope);
                    return RedirectToAction(nameof(Index));  // Redirect to Index after adding
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

           
            return View(boscope);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrEmpty(id))
                return NotFound();

            var scope = await _actscopesetup.GetAllScopes();
            var scopeToEdit = scope.FirstOrDefault(s => s.ScopeId == id);
            if (scopeToEdit == null)
                return NotFound();

            return PartialView(scopeToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ScopeId,ScopeName,ScopeActive")] BocwScope boscope)
        {
            if (id != boscope.ScopeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _actscopesetup.UpdateScope(boscope);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return View(boscope);
        }
    }
}
