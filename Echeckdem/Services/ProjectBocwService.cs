using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Echeckdem.CustomFolder;
using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.ViewModel.ProjectBocw;
using DocumentFormat.OpenXml.InkML;
using Echeckdem.ViewModel.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;



namespace Echeckdem.Services
{
    public class ProjectBocwService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        private readonly HttpContext _httpContext;
        public ProjectBocwService(DbEcheckContext dbEcheckContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbEcheckContext = dbEcheckContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

     //For Project Tracker
        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(string oid, string lcode)
        {
            ProjectDetailsDto projectDetailsDto=new ProjectDetailsDto();
            var locdet = (from locbo in _dbEcheckContext.Ncmlocbos
                          join loc in _dbEcheckContext.Ncmlocs on locbo.Lcode equals loc.Lcode
                          where locbo.Lcode.Trim() == lcode.Trim() && loc.Oid.Trim()==oid.Trim()
                          select new ProjectDetailsDto
                          {
                              Oid = loc.Oid,
                              Lcode = loc.Lcode,
                              SiteName = loc.Lname ?? string.Empty,
                              State = (from st in _dbEcheckContext.Maststates where st.Stateid.Trim() == loc.Lstate.Trim() select st.Statedesc).FirstOrDefault() ?? string.Empty,
                              City = loc.Lcity ?? string.Empty,
                              ClientName = locbo.ClientName ?? string.Empty,
                              GeneralContractor = locbo.GeneralContractor ?? string.Empty,
                              ProjectStartDate = locbo.ProjectStartDateEst.HasValue ? locbo.ProjectStartDateEst.Value.ToString("dd-MMM-yyyy") : string.Empty,
                              ProjectEndDate = locbo.ProjectEndDateEst.HasValue? locbo.ProjectEndDateEst.Value.ToString("dd-MMM-yyyy"):string.Empty,
                              ProjectArea = locbo.ProjectArea,
                              ProjectCost = locbo.ProjectCostEst.HasValue? locbo.ProjectCostEst.Value.ToString("N2"):string.Empty,
                              ProjectLead = locbo.ProjectLead??string.Empty,
                          }).FirstOrDefault();
            projectDetailsDto = locdet;
            if (projectDetailsDto!=null)
            {
                var locationbocw = await (from bocw in _dbEcheckContext.Ncbocws
                                          where bocw.Lcode.Trim() == projectDetailsDto.Lcode.Trim()
                                          orderby bocw.DueDate ascending
                                          select new BocwServiceDto
                                          {
                                              Service = bocw.Task,
                                              ServiceType = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bocw.ScopeId).Select(a => a.ServiceType).FirstOrDefault() ?? string.Empty,
                                              Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bocw.ScopeId).Select(a => a.Category).FirstOrDefault() ?? string.Empty,
                                              DueDate = bocw.DueDate,
                                              Status = _dbEcheckContext.Statusmasters.Where(a => a.ScopeId == bocw.ScopeId && a.Status == bocw.Status).Select(a => a.Value).FirstOrDefault() ?? "Unknown",
                                              CompletionDate = bocw.CompletionDate,
                                              File = bocw.FileName ?? string.Empty,
                                              transactionID = bocw.TransactionId,

                                          }).ToListAsync();
                var locationactivity = await (from action in _dbEcheckContext.Ncactions
                                              where action.Oid.Trim() == projectDetailsDto.Oid.Trim() && action.Lcode.Trim() == projectDetailsDto.Lcode.Trim() && action.Actp.Trim() != "Ops"
                                              select new ComplianceActivityDto
                                              {
                                                  Acid = action.Acid,
                                                  ActivityType = action.Actp,
                                                  Title = action.Actitle ?? string.Empty,
                                                  Desc = action.Acdetail ?? string.Empty,
                                                  StartDate = action.Acidate.HasValue ? action.Acidate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                  CloseDate = action.Accldate.HasValue ? action.Accldate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                                                  Status = action.Acstatus ?? string.Empty,
                                              }
                                              ).ToListAsync();
                projectDetailsDto.BocwServices = locationbocw;
                projectDetailsDto.ProjectActivity = locationactivity;
            }
            // Fetch BOCW data

            return projectDetailsDto;


        }

        //For Compliance Tracker
        public async Task<List<BocwServiceDto>> GetBocwAsync( FilterFormModel model = null)
        {
            if (_httpContext.Session.GetInt32("User Level") == 1)
            {
                if (model == null)
                {
                    return await (from bo in _dbEcheckContext.Ncbocws
                                  join loc in _dbEcheckContext.Ncmlocs on bo.Lcode equals loc.Lcode
                                  join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid

                                  where loc.Lactive == 1 && org.Oactive == 1

                                  orderby bo.DueDate ascending
                                  select new BocwServiceDto
                                  {
                                      Id = bo.TransactionId,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.ServiceType).FirstOrDefault() ?? string.Empty,
                                      Service = bo.Task,
                                      Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.Category).FirstOrDefault() ?? string.Empty,
                                      DueDate = bo.DueDate,
                                      Status = _dbEcheckContext.Statusmasters.Where(a => a.ScopeId == bo.ScopeId && a.Status == bo.Status).Select(a => a.Value).FirstOrDefault() ?? "Unknown",
                                      CompletionDate = bo.CompletionDate,
                                      File = bo.FileName ?? string.Empty,
                                      transactionID = bo.TransactionId
                                  }).ToListAsync();

                }
                else
                {
                    return await (from bo in _dbEcheckContext.Ncbocws
                                  join loc in _dbEcheckContext.Ncmlocs on bo.Lcode equals loc.Lcode
                                  join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid

                                  where loc.Lactive == 1 && org.Oactive == 1
                                  && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid.Trim() == model.SelectedClient.Trim())
                                           && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode == model.SelectedSite)
                                            && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                            && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                            && (model.StartDueDate == null || bo.DueDate >= model.StartDueDate)
                                            && (model.EndDueDate == null || bo.DueDate <= model.EndDueDate)

                                  orderby bo.DueDate ascending
                                  select new BocwServiceDto
                                  {
                                      Id = bo.TransactionId,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.ServiceType).FirstOrDefault() ?? string.Empty,
                                      Service = bo.Task,
                                      Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.Category).FirstOrDefault() ?? string.Empty,
                                      DueDate = bo.DueDate,
                                      Status = _dbEcheckContext.Statusmasters.Where(a => a.ScopeId == bo.ScopeId && a.Status == bo.Status).Select(a => a.Value).FirstOrDefault() ?? "Unknown",
                                      CompletionDate = bo.CompletionDate,
                                      File = bo.FileName ?? string.Empty,
                                      transactionID = bo.TransactionId
                                  }).ToListAsync();
                }
            }
            else
            {
                var uno = _httpContext.Session.GetInt32("UNO");

                if (model == null)
                {
                    return await (from bo in _dbEcheckContext.Ncbocws
                                  join loc in _dbEcheckContext.Ncmlocs on bo.Lcode equals loc.Lcode
                                  join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                  join usem in _dbEcheckContext.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                  where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1

                                  orderby bo.DueDate ascending
                                  select new BocwServiceDto
                                  {
                                      Id = bo.TransactionId,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.ServiceType).FirstOrDefault() ?? string.Empty,
                                      Service = bo.Task,
                                      Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.Category).FirstOrDefault() ?? string.Empty,
                                      DueDate = bo.DueDate,
                                      Status = _dbEcheckContext.Statusmasters.Where(a => a.ScopeId == bo.ScopeId && a.Status == bo.Status).Select(a => a.Value).FirstOrDefault() ?? "Unknown",
                                      CompletionDate = bo.CompletionDate,
                                      File = bo.FileName ?? string.Empty,
                                      transactionID = bo.TransactionId
                                  }).ToListAsync();

                }
                else
                {
                    Console.WriteLine($"-----------------------\n--------uno{uno}---------------Client: {model?.SelectedClient}, Site: {model?.SelectedSite}, State: {model?.SelectedState}, City: {model?.SelectedCity}---------------------\n---------------{_dbEcheckContext.Ncmorgs.Where(x=>x.Oid==model.SelectedClient).Select(x => x.Oname).FirstOrDefault()}-\n-------{_dbEcheckContext.Ncmlocs.Where(x => x.Lcode == model.SelectedSite).Select(x => x.Lname).FirstOrDefault()}----");

                    var result= await (from bo in _dbEcheckContext.Ncbocws
                                  join loc in _dbEcheckContext.Ncmlocs on bo.Lcode equals loc.Lcode
                                  join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                  join usem in _dbEcheckContext.Ncumaps on new { loc.Oid, loc.Lcode } equals new { usem.Oid, usem.Lcode }
                                  where usem.Uno == uno && loc.Lactive == 1 && org.Oactive == 1
                                 && (string.IsNullOrEmpty(model.SelectedClient) || org.Oid.Trim() == model.SelectedClient.Trim())
                                           && (string.IsNullOrEmpty(model.SelectedSite) || loc.Lcode.Trim()== model.SelectedSite.Trim())
                                            && (string.IsNullOrEmpty(model.SelectedState) || loc.Lstate == model.SelectedState)
                                            && (string.IsNullOrEmpty(model.SelectedCity) || loc.Lcity == model.SelectedCity)
                                            && (model.StartDueDate == null || bo.DueDate >= model.StartDueDate)
                                            && (model.EndDueDate == null || bo.DueDate <= model.EndDueDate)

                                  orderby bo.DueDate ascending
                                  select new BocwServiceDto
                                  {
                                      Id = bo.TransactionId,
                                      Oid = org.Oid,
                                      Lcode = loc.Lcode,
                                      Oname = org.Oname,
                                      Lname = loc.Lname,
                                      Lcity = loc.Lcity,
                                      Lstate = loc.Lstate,
                                      ServiceType = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.ServiceType).FirstOrDefault() ?? string.Empty,
                                      Service = bo.Task,
                                      Category = _dbEcheckContext.BocwScopes.Where(a => a.ScopeId == bo.ScopeId).Select(a => a.Category).FirstOrDefault() ?? string.Empty,
                                      DueDate = bo.DueDate,
                                      Status = _dbEcheckContext.Statusmasters.Where(a => a.ScopeId == bo.ScopeId && a.Status == bo.Status).Select(a => a.Value).FirstOrDefault() ?? "Unknown",
                                      CompletionDate = bo.CompletionDate,
                                      File = bo.FileName ?? string.Empty,
                                      transactionID = bo.TransactionId
                                  }).ToListAsync();
                    Console.WriteLine($"----------\n{result}-------------\n");
                    return result;

                }
            }
                

        }

        //For Compliance Tracker+ Project Tracker=> Opening Single NCBOCW record
        public async Task<BOCWEditViewModel> GetEditData(string transactionId)
        {
            int id = string.IsNullOrEmpty(transactionId) ? 0 : Convert.ToInt32(transactionId);
            BOCWEditViewModel mojo = new BOCWEditViewModel();

            mojo.ProjectSummary = await (from bocw in _dbEcheckContext.Ncbocws
                                         join loc in _dbEcheckContext.Ncmlocs on bocw.Lcode equals loc.Lcode
                                         join locbo in _dbEcheckContext.Ncmlocbos on loc.Lcode equals locbo.Lcode
                                         join org in _dbEcheckContext.Ncmorgs on loc.Oid equals org.Oid
                                         where bocw.TransactionId == id
                                         select new ProjectSummary {
                                         Oid= org.Oid ?? string.Empty,
                                             Orgname = org.Oname ?? string.Empty,
                                             Lcode = loc.Lcode ?? string.Empty,
                                             SiteName = loc.Lname ?? string.Empty,
                                             State = (from st in _dbEcheckContext.Maststates where st.Stateid.Trim() == loc.Lstate.Trim() select st.Statedesc).FirstOrDefault() ?? string.Empty,
                                             City = loc.Lcity ?? string.Empty,
                                             ClientName = locbo.ClientName ?? string.Empty,
                                             ProjectLead = locbo.ProjectLead ?? string.Empty
                                         }).FirstOrDefaultAsync();


            mojo.Task = await (from bocw in _dbEcheckContext.Ncbocws where bocw.TransactionId==id
                               select new NcbocwUpdateModel { 
                               TransactionId=bocw.TransactionId,
                                   Title = bocw.Task,
                                   DueDate = bocw.DueDate,
                                   Status = bocw.Status,
                                   CompletionDate = bocw.CompletionDate,
                                   ExistingFileName = bocw.FileName,
                                   ScopeId = bocw.ScopeId
                               }).FirstOrDefaultAsync(); ;

            mojo.Ncaction = _dbEcheckContext.Ncactions.Where(x=>x.Aclink==id && x.Actp=="Ops").Select(x => new BOCWNcactionEdit 
            {
                Acid = x.Acid,
                Oid = mojo.ProjectSummary.Oid??string.Empty,
                Acdetail = x.Acdetail,
                Acremarks = x.Acremarks,
                Acidate = x.Acidate.HasValue?x.Acidate.Value : DateOnly.FromDateTime(DateTime.Now),
                TransactionId = id
            }).FirstOrDefault();
            
            if (mojo.Ncaction != null)
                mojo.ncactakens = (from taken in _dbEcheckContext.Ncactakens
                                   join usr in _dbEcheckContext.Ncusers on taken.Uno equals usr.Uno
                                   where taken.Acid==mojo.Ncaction.Acid
                                   select new BocwNcactakensum
                                   {
                                   Acid = taken.Acid,
                                       Actid = taken.Actid,
                                       Acdate = taken.Acdate,
                                       Actaken = taken.Actaken,
                                       Nacdate = taken.Nacdate,
                                       
                                       Accrdate = taken.Accrdate,
                                       Uname = usr.Uname
                                   }).ToList();
            if (mojo.Ncaction != null)
                mojo.Ncaction.ExistingFileName=_dbEcheckContext.Ncfiles.Where(x => x.Flink == mojo.Ncaction.Acid && x.Ftp.Trim()=="Act").Select(x=>x.Fname).ToList();
            mojo.Ncactaken.Acid = mojo.Ncaction.Acid;   
            mojo.Ncactaken.TransactionId = id;  

            var statusOptions = _dbEcheckContext.Statusmasters.Where(s => s.ScopeId == mojo.Task.ScopeId && s.Active == 1)
                .Select(s => new SelectListItem
                {
                    Value = s.Status.ToString(),
                    Text = s.Value
                })
                .ToList();
            mojo.Task.AvailableStatuses = statusOptions;








            return mojo;
        }

        


    }
}
