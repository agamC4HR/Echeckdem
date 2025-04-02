using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.Services
{

    public class TrackerService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly ILogger<TrackerService> _logger;

        public TrackerService(DbEcheckContext dbEcheckContext, ILogger<TrackerService> logger)
        {
            _dbEcheckContext = dbEcheckContext;
            _logger = logger;
        }

        public int GetUnoFromSession(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UNO") ?? 0;
        }

        public List<SelectListItem> GetOrganizations(int uno)
        {
            return _dbEcheckContext.Ncumaps
        .Where(x => x.Uno == uno)
        .Select(x => x.Oid) 
        .Distinct() 
        .Join(_dbEcheckContext.Ncmorgs, oid => oid, org => org.Oid,
            (oid, org) => new SelectListItem
            {
                Value = org.Oid.ToString(),
                Text = org.Oname
            })
        .ToList();
        }

        public List<SelectListItem> GetLocations(int uno, string oid)
        {
            var locations = _dbEcheckContext.Ncumaps
                .Where(x => x.Uno == uno && x.Oid == oid)
                .Join(_dbEcheckContext.Ncmlocs, umap => umap.Lcode, org => org.Lcode,
                    (umap, org) => new SelectListItem
                    {
                        Value = org.Lcode,
                        Text = org.Lname
                    }).ToList();

            // Check if locations are being fetched correctly
          if (!locations.Any())
{
    _logger.LogWarning($"No locations found for UNO: {uno}, OID: {oid}");
}

            return locations;
        }

        public List<SelectListItem> GetTPPDropdown()
        {
            var tppList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Labour", Text = "Labour" },
        new SelectListItem { Value = "PF", Text = "PF" },
        new SelectListItem { Value = "ESI", Text = "ESI" }
    };

            // Ensure no duplicates by using Distinct() if necessary
            return tppList.DistinctBy(item => item.Value).ToList();  // Using DistinctBy to ensure uniqueness
        }

        public List<SelectListItem> GetActDropdown()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "SE", Text = "Shops & Estb" },
            new SelectListItem { Value = "CLRA", Text = "Contract Labour" },
            new SelectListItem { Value = "PoW", Text = "Payment of Wages" },
            new SelectListItem { Value = "PoB", Text = "Payment of Bonus" },
            new SelectListItem { Value = "MW", Text = "Minimum Wages" },
            new SelectListItem { Value = "PF", Text = "Provident Fund" },
            new SelectListItem { Value = "ESI", Text = "Emp. state Insurance" },
            new SelectListItem { Value = "NFH", Text = "National Festival Holidays" },
            new SelectListItem { Value = "PoG", Text = "Payment of Gratuity" },
            new SelectListItem { Value = "MB", Text = "Maternity Benefit" },
            new SelectListItem { Value = "ER-I", Text = "Employment Exchange" },
            new SelectListItem { Value = "OT", Text = "Other" }
        };
        }

        public List<SelectListItem> GetSlaDropdown()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "INL", Text = "Inspection/Notice[Labour]" },
            new SelectListItem { Value = "INPFESI", Text = "Inspection/Notice[PF/ESI]" }
        };
        }

        public void SaveNcAction(TrackerViewModel model)
        {
            var newRecord = new Ncaction
            {
                Tpp = model.SelectedTPP,
                Actitle = model.SelectedACTITLE,
                Sbtp = model.SelectedSBTP
            };

            _dbEcheckContext.Ncactions.Add(newRecord);
            _dbEcheckContext.SaveChanges();
        }

      


    }
}
