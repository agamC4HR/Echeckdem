using Echeckdem.Models;
using Echeckdem.ViewModel.OnGoingActivity;
using Echeckdem.ViewModel.ProjectBocw;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Echeckdem.Services
{

    public class TrackerService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly ILogger<TrackerService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpContext _httpContext;
        private static readonly Dictionary<string, string> StatusDescriptions = new()
{
            { "","Yet To Start"} ,
    { "O", "Open" },
    { "I", "Docs received, In Process" },
    { "D", "Docs requested" },
    { "C", "Closed" }
    

};
        private static readonly Dictionary<string, string> IStatusDescriptions = new()
{
            { "","unknown"} ,
    { "N", "Normal" },
    { "P", "Priority" },
    { "E", "Escalated" },
    { "C", "Critical" }


};
        private static readonly Dictionary<string, string> ActivityTypeDescriptions = new()
{
            { "","Unknown"} ,
    { "O", "Other" },
    { "I", "Inspection/Notice" },
    { "P", "PF Queries" },
    { "A", "Registration New/Amendment" },
    { "S", "Site Visit" },
    { "C", "Client Audit" }


};
        private static readonly Dictionary<string, string> ActDescriptions = new()
{
               { "BOCW", "BOCW Act" },
                 { "BOWC", "BOCW Cess Act" },
                 { "SE", "Shops & Estbt. Act" },
                 { "CLRA", "Contract Labour Regulation Act" },
                 { "PoW", "Payment of Wages Act" },
                 { "PoB", "Payment of Bonus Act" },
                 { "MW", "Minimum Wages Act" },
                 { "PF", "Provident Fund Act" },
                 { "ESI", "Employee state Insurance Act" },
                 { "NFH", "National Festival Holidays Act" },
                 { "PoG", "Payment of Gratuity Act" },
                 { "MB", "Maternity Benefit Act" },
                 { "EE", "Employment Exchange Act" },
                 { "OA", "Other Act" }


};
        public TrackerService(DbEcheckContext dbEcheckContext, ILogger<TrackerService> logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _dbEcheckContext = dbEcheckContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string GetActivityName(string tp) 
        {
        return ActivityTypeDescriptions.TryGetValue(tp?.Trim() ?? "", out var activityType) ? activityType : "Unknown";
        }
        public string GetActName(string tp)
        {
            return ActDescriptions.TryGetValue(tp?.Trim() ?? "", out var actType) ? actType : "Unknown";
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
                new SelectListItem { Value = "BOCW", Text = "BOCW Act" },
                new SelectListItem { Value = "BOWC", Text = "BOCW Cess Act" },
                new SelectListItem { Value = "SE", Text = "Shops & Estbt. Act" },
                new SelectListItem { Value = "CLRA", Text = "Contract Labour Regulation Act" },
                new SelectListItem { Value = "PoW", Text = "Payment of Wages Act" },
                new SelectListItem { Value = "PoB", Text = "Payment of Bonus Act" },
                new SelectListItem { Value = "MW", Text = "Minimum Wages Act" },
                new SelectListItem { Value = "PF", Text = "Provident Fund Act" },
                new SelectListItem { Value = "ESI", Text = "Employee state Insurance Act" },
                new SelectListItem { Value = "NFH", Text = "National Festival Holidays Act" },
                new SelectListItem { Value = "PoG", Text = "Payment of Gratuity Act" },
                new SelectListItem { Value = "MB", Text = "Maternity Benefit Act" },
                new SelectListItem { Value = "EE", Text = "Employment Exchange Act" },
                new SelectListItem { Value = "OA", Text = "Other Act" }
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

        public List<SelectListItem> GetACTPDropdown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Registration New/Amendment" },
                new SelectListItem { Value = "O", Text = "Other" },
                new SelectListItem { Value = "P", Text = "PF Queries" },
                new SelectListItem { Value = "I", Text = "Inspection/Notice" },
                new SelectListItem { Value = "S", Text = "Site Visit"},
                new SelectListItem { Value = "C", Text = "Client Audit"}
            };
        }

        //----------------------------START-------------------Fetching Actions for Tracker List------------------------------------------------------------------------
        public List<TrackerViewModel> GetNcActionsForUser()
        {
            if (_httpContext.Session.GetInt32("User Level") == 1)
            {
                

                var ncaction = (from act in _dbEcheckContext.Ncactions
                                join loc in _dbEcheckContext.Ncmlocs on act.Lcode equals loc.Lcode
                                join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                
                                where  loc.Lactive == 1 && org.Oactive == 1 && !string.IsNullOrEmpty(act.Actp) && act.Actp.Trim() != "Ops"
                                select new { act, loc, org }).ToList();



                return ncaction.Select(x => new TrackerViewModel
                {
                    Acid = x.act.Acid,
                    Oname = x.org.Oname,
                    Lname = x.loc.Lname,
                    Title = x.act.Actitle,
                    ExternalStatus = StatusDescriptions.TryGetValue(x.act.Acstatus?.Trim() ?? "", out var status) ? status : "Unknown",
                    DetailOfIssue = x.act.Acdetail,
                    StartDate = x.act.Acidate,
                    CloseDate = x.act.Accldate,
                    ActivityType = ActivityTypeDescriptions.TryGetValue(x.act.Actp?.Trim() ?? "", out var activityType) ? activityType : "Unknown",
                }).ToList();
            }
            else
            {
                var uno = _httpContext.Session.GetInt32("UNO");

                var ncaction = (from act in _dbEcheckContext.Ncactions
                                join loc in _dbEcheckContext.Ncmlocs on act.Lcode equals loc.Lcode
                                join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                join userm in _dbEcheckContext.Ncumaps on new { loc.Oid, loc.Lcode } equals new { userm.Oid, userm.Lcode }
                                where userm.Uno == uno && loc.Lactive == 1 && org.Oactive == 1 && !string.IsNullOrEmpty(act.Actp) && act.Actp.Trim() != "Ops"
                                select new { act, loc, org, userm }).ToList();



                return ncaction.Select(x => new TrackerViewModel
                {
                    Acid = x.act.Acid,
                    Oname = x.org.Oname,
                    Lname = x.loc.Lname,
                    Title = x.act.Actitle,
                    ExternalStatus = StatusDescriptions.TryGetValue(x.act.Acstatus?.Trim() ?? "", out var status) ? status : "Unknown",
                    DetailOfIssue = x.act.Acdetail,
                    StartDate = x.act.Acidate,
                    CloseDate = x.act.Accldate,
                    ActivityType = ActivityTypeDescriptions.TryGetValue(x.act.Actp?.Trim() ?? "", out var activityType) ? activityType : "Unknown",
                }).ToList();
            }
                
        }

        //------------------Fetching Action Details for ACID------------------------------------------------------------------------
        public async Task<TrackerFullViewModel> GetNcActionDet(string Acid) 
        {
            int id = string.IsNullOrEmpty(Acid) ? 0 : Convert.ToInt32(Acid);
            TrackerFullViewModel model = new TrackerFullViewModel();
            var actiondata = await (from act in _dbEcheckContext.Ncactions
                                    join loc in _dbEcheckContext.Ncmlocs on act.Lcode equals loc.Lcode
                                    join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                    where act.Acid == id
                                    select new { act, loc, org }).FirstOrDefaultAsync();

            if (actiondata != null)
            {
                model = new TrackerFullViewModel
                {
                    Acid = actiondata.act.Acid,
                    Oid = actiondata.org.Oid,
                    Title = actiondata.act.Actitle,
                    Acstatus =actiondata.act.Acstatus??"",// StatusDescriptions.TryGetValue(actiondata.act.Acstatus?.Trim() ?? "", out var status) ? status : "Unknown",
                    Acistatus = actiondata.act.Acistatus ?? "",   //IStatusDescriptions.TryGetValue(actiondata.act.Acistatus?.Trim() ?? "", out var istatus) ? istatus : "Unknown",
                    Acdetail = actiondata.act.Acdetail ?? string.Empty,
                    Acidate = actiondata.act.Acidate,
                    Adocdate = actiondata.act.Adocdate,
                    Accldate = actiondata.act.Accldate,
                    Remark = actiondata.act.Acremarks ?? string.Empty,
                    Oname = actiondata.org.Oname,
                    Lname = actiondata.loc.Lname,
                    Acrdate = actiondata.act.Acrdate,
                    ActivityType =ActivityTypeDescriptions.TryGetValue(actiondata.act.Actp?.Trim()??"",out var actype)?actype:"Unknown",
                    Uname=_dbEcheckContext.Ncusers.Where(x => x.Uno == actiondata.act.Acruser).Select(x => x.Uname).FirstOrDefault()
                };
            model.TakenViewModel= (from taken in _dbEcheckContext.Ncactakens
                                   join usr in _dbEcheckContext.Ncusers on taken.Uno equals usr.Uno
                                   where taken.Acid == model.Acid
                                   select new TrackerTakenViewModel
                                   {
                                       Acid = taken.Acid,
                                       Actid = taken.Actid,
                                       Acdate = taken.Acdate,
                                       Actaken = taken.Actaken??string.Empty,
                                       Nacdate = taken.Nacdate,

                                       Acrdate = taken.Accrdate,
                                       Uname = usr.Uname
                                   }).ToList();
            }
            model.Taken.Acid= model.Acid;
            return model;
        }


       

    }
}
