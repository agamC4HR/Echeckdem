using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.Services
{

    public class TrackerService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly ILogger<TrackerService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TrackerService(DbEcheckContext dbEcheckContext, ILogger<TrackerService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbEcheckContext = dbEcheckContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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

           
            return tppList.DistinctBy(item => item.Value).ToList();  
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
        public List<TrackerViewModel> GetNcActionsForUser(int uno)
        {
            // Step 1: Fetch OID and LCODE mappings for the user
            var userMappings = _dbEcheckContext.Ncumaps
                .Where(x => x.Uno == uno && !string.IsNullOrEmpty(x.Oid) && !string.IsNullOrEmpty(x.Lcode))
                .Select(x => new { x.Oid, x.Lcode })
                .ToList();

            if (!userMappings.Any())
            {
                _logger.LogWarning($"No organizations found for user UNO: {uno}");
                return new List<TrackerViewModel>();
            }

            // Step 2: Fetch all Ncactions first (before filtering)
            var allNcActions = _dbEcheckContext.Ncactions
                .ToList(); // Convert to List before filtering

            // Step 3: Filter in-memory
            var filteredActions = allNcActions
                .Where(x => userMappings.Any(m => m.Oid == x.Oid && m.Lcode == x.Lcode))
                .ToList();

            // Step 4: Fetch organization names
            var orgs = _dbEcheckContext.Ncmorgs.ToList(); // Fetch all orgs
            var locs = _dbEcheckContext.Ncmlocs.ToList(); // Fetch all locations

            // Step 5: Map to ViewModel with Full Names
            var ncActions = filteredActions
                .Select(action => new TrackerViewModel
                {
                    Acid = action.Acid,
                    Oname = orgs.FirstOrDefault(o => o.Oid == action.Oid)?.Oname ?? "Unknown Org",
                    Lname = locs.FirstOrDefault(l => l.Lcode == action.Lcode)?.Lname ?? "Unknown Location",
                    SelectedTPP = action.Tpp,
                    SelectedACTITLE = action.Actitle,
                    SelectedSBTP = action.Sbtp
                })
                .ToList();

            return ncActions;
        }



        public void SaveNcAction(TrackerViewModel model)
        {
            var newRecord = new Ncaction
            {
                Acid = model.Acid,
                Oid = model.SelectedOid,   
                Lcode = model.SelectedLCODE,
                Tpp = model.SelectedTPP,
                Actitle = model.SelectedACTITLE,
                Sbtp = model.SelectedSBTP
            };

            _dbEcheckContext.Ncactions.Add(newRecord);
            _dbEcheckContext.SaveChanges();
        }

        public void SaveNcFile(int acid, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file.");

            var action = _dbEcheckContext.Ncactions.FirstOrDefault(a => a.Acid == acid);
            if (action == null)
                throw new Exception("Action not found.");

            string oid = action.Oid.ToString();
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid, "Acts");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var ncFile = new Ncfile
            {
                Oid = action.Oid,
                Flink = action.Acid,  // Same as ACID
                Fname = fileName,
                Fupdate = DateOnly.FromDateTime(DateTime.Now)
            };

            _dbEcheckContext.Ncfiles.Add(ncFile);
            _dbEcheckContext.SaveChanges();
        }


        public TrackerViewModel GetNcActionById(int acid)
        {
            var action = _dbEcheckContext.Ncactions.FirstOrDefault(a => a.Acid == acid);
            if (action == null) 
                return null;

            return new TrackerViewModel
            {
                Acid = action.Acid,
                Title = action.Actitle,
                ExternalStatus = action.Acstatus,
                VisibleToClient = action.Acshow ?? 0,
                InternalStatus = action.Acistatus,
                DetailOfIssue = action.Acdetail,
                StartDate = action.Acidate,
                DocsReceiptDate = action.Adocdate,
                CloseDate = action.Accldate,
                Remarks = action.Acremarks
            };
        }

        public void SaveOrUpdateNcAction(TrackerViewModel model)
        {
            var existingAction = _dbEcheckContext.Ncactions.FirstOrDefault(a => a.Acid == model.Acid);

            if (existingAction != null)
            {
                existingAction.Actitle = model.Title;
                existingAction.Acstatus = model.ExternalStatus;
                existingAction.Acshow = model.VisibleToClient;
                existingAction.Acistatus = model.InternalStatus;
                existingAction.Acdetail = model.DetailOfIssue;
                existingAction.Acidate = model.StartDate;
                existingAction.Adocdate = model.DocsReceiptDate;
                existingAction.Accldate = model.CloseDate;
                existingAction.Acremarks = model.Remarks;
            }
            else
            {
                var newAction = new Ncaction
                {
                    Actitle = model.Title,
                    Acstatus = model.ExternalStatus,
                    Acshow = model.VisibleToClient,
                    Acistatus = model.InternalStatus,
                    Acdetail = model.DetailOfIssue,
                    Acidate = model.StartDate,
                    Adocdate = model.DocsReceiptDate,
                    Accldate = model.CloseDate,
                    Acremarks = model.Remarks
                };
                _dbEcheckContext.Ncactions.Add(newAction);
            }

            _dbEcheckContext.SaveChanges();
        }


        public TrackerTakenViewModel GetNcActTakenByAcid(int acid)
        {
            var actionTaken = _dbEcheckContext.Ncactakens.FirstOrDefault(a => a.Acid == acid);

            if (actionTaken == null)
            {
                return new TrackerTakenViewModel
                {
                    Acid = acid,
                    Actid = 0, // 0 indicates a new entry
                    Acdate = null,
                    Actaken = string.Empty,
                    Nacdate = null,
                    Showclient = 0
                };
            }

            return new TrackerTakenViewModel
            {
                Actid = actionTaken.Actid,
                Acid = actionTaken.Acid,
                Acdate = actionTaken.Acdate,
                Actaken = actionTaken.Actaken,
                Nacdate = actionTaken.Nacdate,
                Showclient = actionTaken.Showclient ?? 0
            };

        }

        public void SaveOrUpdateNcActTaken(TrackerTakenViewModel model)
        {
            var existingActionTaken = _dbEcheckContext.Ncactakens.FirstOrDefault(a => a.Actid == model.Actid);

            if (existingActionTaken != null)
            {
                existingActionTaken.Acdate = model.Acdate;
                existingActionTaken.Actaken = model.Actaken;
                existingActionTaken.Nacdate = model.Nacdate;
                existingActionTaken.Showclient = model.Showclient;
            }
            else
            {
                var newActionTaken = new Ncactaken
                {
                    Acid = model.Acid,
                    Acdate = model.Acdate,
                    Actaken = model.Actaken,
                    Nacdate = model.Nacdate,
                    Showclient = model.Showclient
                };
                _dbEcheckContext.Ncactakens.Add(newActionTaken);
            }

            _dbEcheckContext.SaveChanges();
        }





    }
}
