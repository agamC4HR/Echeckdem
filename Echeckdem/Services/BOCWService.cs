using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;


namespace Echeckdem.Services
{
    public class BOCWService
    {
        private readonly DbEcheckContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BOCWService(DbEcheckContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BocwViewModel>> GetDataAsync(int ulev, int uno, string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {

            var currentYear = DateTime.Now.Year;
            var sqlQuery = @"
                            SELECT a.lcode, a.lname, a.ProjectCode, a.ScopeID, a.DueDate, a.Status, a.Task, a.CreateDate, a.TransactionID,
                             b.lstate, b.lcity, b.lregion, b.oid,
                            c.oname,
                           d.statedesc AS State,
                            f.FName AS Filename

                            FROM ncbocw a
                            JOIN ncmloc b ON a.lcode = b.lcode
                            JOIN ncmorg c ON c.oid = b.oid
                            JOIN MASTSTATES d ON b.lstate = d.stateid
                            LEFT JOIN ncaction ac ON ac.aclink = a.TransactionID
                            LEFT JOIN (
                            SELECT Oid, Flink, FName,
                            ROW_NUMBER() OVER (PARTITION BY Flink ORDER BY Fupdate DESC) AS rn
                            FROM NCFILES
                            WHERE FName IS NOT NULL
                            ) f ON f.Flink = ac.Acid AND f.Oid = b.oid AND f.rn = 1

                            WHERE c.oactive = 1 
                            AND (a.status IS NULL OR a.status <> 99)

                                ";

            //SELECT a.lcode, a.lname, a.ProjectCode, a.ScopeID, a.DueDate, a.Status, a.Task, a.CreateDate, a.TransactionID,
            //                    b.lstate, b.lcity, b.lregion,
            //                    c.oname,                
            //                    d.statedesc as State

            //                    FROM ncbocw a
            //                    JOIN ncmloc b ON a.lcode = b.lcode
            //                    JOIN ncmorg c ON c.oid = b.oid
            //                    JOIN MASTSTATES d ON b.lstate = d.stateid
            //                    WHERE c.oactive = 1


            //                    AND a.lcode = b.lcode
            //                    AND(a.status IS NULL OR a.status<> 99)

            if (string.IsNullOrEmpty(organizationName) &&
                string.IsNullOrEmpty(LocationName) &&
                string.IsNullOrEmpty(StateName) &&
                string.IsNullOrEmpty(CityName))
            {
                sqlQuery += "AND YEAR(a.DueDate) = @currentYear ";
            }
            // Add extra filtering if ulev > 1 
            if (ulev >= 1)
            {
                sqlQuery += @" AND b.oid IN (SELECT DISTINCT oid FROM ncumap WHERE uno = @uno)
                           AND b.lactive = '1'
                           AND a.lcode IN (SELECT DISTINCT lcode FROM ncumap WHERE uno = @uno)";
            }
            // applying filters
            if (!string.IsNullOrEmpty(organizationName))
            {
                sqlQuery += " AND c.oname = @organizationName";
            }
            if (!string.IsNullOrEmpty(LocationName))
            {
                sqlQuery += " AND a.lname = @LocationName";
            }

            if (!string.IsNullOrEmpty(StateName))
            {
                sqlQuery += " AND d.statedesc = @StateName";
            }

            if (!string.IsNullOrEmpty(CityName))
            {
                sqlQuery += " AND b.lcity = @CityName";
            }
            if (StartDueDate.HasValue)
            {
                sqlQuery += " AND a.DueDate >= @StartDueDate";
            }
            if (EndDueDate.HasValue)
            {
                sqlQuery += " AND a.DueDate <= @EndDueDate";
            }
            if (StartPeriod.HasValue)
            {
                sqlQuery += " AND a.Period >= @StartPeriod";
            }
            if (EndPeriod.HasValue)
            {
                sqlQuery += " AND a.Period <= @EndPeriod";
            }

            sqlQuery += " ORDER BY a.DueDate DESC, a.lname";

            // Execute the SQL query 
            var result = await _context.BocwViewModel
                 .FromSqlRaw(sqlQuery,
                             new SqlParameter("@currentYear", currentYear),
                             new SqlParameter("@uno", uno),
                             new SqlParameter("@organizationName", (object)organizationName ?? DBNull.Value),
                             new SqlParameter("@LocationName", (object)LocationName ?? DBNull.Value),
                             new SqlParameter("@StateName", (object)StateName ?? DBNull.Value),
                             new SqlParameter("@CityName", (object)CityName ?? DBNull.Value),
                             new SqlParameter("@StartDueDate", (object)StartDueDate ?? DBNull.Value),
                             new SqlParameter("@EndDueDate", (object)EndDueDate ?? DBNull.Value),
                             new SqlParameter("@StartPeriod", (object)StartPeriod ?? DBNull.Value),
                             new SqlParameter("@EndPeriod", (object)EndPeriod ?? DBNull.Value))
                .ToListAsync();
            return result;
        }

        public async Task<List<string>> GetLocationNamesAsync(int uno)         // Get all locations name for filters
        {
            var sqlQuery = @"
            SELECT DISTINCT b.Lname
            FROM NCUMAP m
            JOIN NCMLOC b on m.Lcode = b.Lcode 
            WHERE m.Uno = {0} AND b.Lactive = 1";

            var LocationNames = await _context.Ncmlocs
            .FromSqlRaw(sqlQuery, uno)
            .Select(l => l.Lname ?? string.Empty)
            .Where(l => !string.IsNullOrEmpty(l))
            .ToListAsync();

            return LocationNames;
        }
        public BOCWEditViewModel GetEditData(string lcode, int transactionId)
        {
            var bocw = _context.Ncbocws.FirstOrDefault(x => x.Lcode == lcode && x.TransactionId == transactionId);
            var action = _context.Ncactions.FirstOrDefault(x => x.Aclink == transactionId);
           

            if (bocw == null || action == null) return null;

            var scopeId = bocw.ScopeId;

            var statusOptions = _context.Statusmasters.Where(s => s.ScopeId == scopeId && s.Active == 1)
                .Select(s => new SelectListItem
                {
                    Value = s.Status.ToString(),
                    Text = s.Value
                })
                .ToList();

            var model = new BOCWEditViewModel //return new BOCWEditViewModel
            {
                LCode = lcode,
                TransactionID = transactionId,
                DueDate = bocw.DueDate,
                Status = bocw.Status,
                CompletionDate = bocw.CompletionDate,

                ACID = action.Acid,
                ACTitle = action.Actitle,
                ACDetail = action.Acdetail,
                ACIDate = action.Acidate,
                ACShow = action.Acshow,
                ACStatus = action.Acstatus,
                ACRDate = action.Acrdate,
                ACRemarks = action.Acremarks,
                
                AvailableStatuses = statusOptions
            };

            var ncactaken = _context.Ncactakens.FirstOrDefault(x=>x.Acid == action.Acid);
            if (ncactaken != null)
            {
                
                model.ActionTaken = ncactaken.Actaken;
                model.ActionDate = ncactaken.Acdate;
                model.ActionClosedDate = ncactaken.Nacdate;
                model.ShowClient = ncactaken.Showclient;
            }

            return model;
        }

        public void UpdateOnlyNCBOCW(BOCWEditViewModel model)
        {
            var bocw = _context.Ncbocws.FirstOrDefault(x => x.Lcode == model.LCode && x.TransactionId == model.TransactionID);
            if (bocw != null)
            {
                bocw.Status = model.Status;
                bocw.CompletionDate = model.CompletionDate;
            }
            _context.SaveChanges();
        }

        public void UpdateOnlyNCACTION(BOCWEditViewModel model)
        {
            var action = _context.Ncactions.FirstOrDefault(x => x.Acid == model.ACID);
            if (action != null)
            {
                action.Acdetail = model.ACDetail;
                action.Acshow = model.ACShow;
                action.Acremarks = model.ACRemarks;
                action.Acidate = model.ACIDate;
            }

            var ncactaken = _context.Ncactakens.FirstOrDefault(x => x.Acid == model.ACID);
            var httpContext = _httpContextAccessor.HttpContext;
            var uno = httpContext?.Session.GetInt32("UNO");

            if (uno == null)
            {
                throw new InvalidOperationException("UNO not found in session.");
            }

            if (ncactaken == null)
            {
                // Insert new
                ncactaken = new Ncactaken
                {
                    Acid = model.ACID,
                    Actaken = model.ActionTaken,
                    Acdate = model.ActionDate,
                    Nacdate = model.ActionClosedDate,
                    Uno = uno.Value,
                    Showclient = model.ShowClient
                };
                _context.Ncactakens.Add(ncactaken);
            }
            else
            {
                // Update existing
                ncactaken.Actaken = model.ActionTaken;
                ncactaken.Acdate = model.ActionDate;
                ncactaken.Nacdate = model.ActionClosedDate;
                ncactaken.Showclient = model.ShowClient;
            }

            if (model.UploadedFile != null)
            {
                var file = model.UploadedFile;

                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    throw new InvalidOperationException("Only PDF files are allowed.");
                }

                var oid = action?.Oid;
                string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "Bocw");
                Directory.CreateDirectory(folderPath);

                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var ncFile = new Ncfile
                {
                    Oid = oid,
                    Flink = action.Acid,
                    Fname = fileName,
                    Fupdate = DateOnly.FromDateTime(DateTime.Today)
                };

                _context.Ncfiles.Add(ncFile);
            }

            _context.SaveChanges();
        }

        //public void UpdateOnlyNCACTION(BOCWEditViewModel model)  
        //{
        //    var action = _context.Ncactions.FirstOrDefault(x => x.Acid == model.ACID);
        //    if (action != null)
        //    {
        //        action.Acdetail = model.ACDetail;
        //        action.Acshow = model.ACShow;
        //        action.Acremarks = model.ACRemarks;
        //        action.Acidate = model.ACIDate;

        //        // Insert into NCACTAKEN if not exists
        //        var alreadyExists = _context.Ncactakens.Any(x => x.Acid == model.ACID);
        //        if (!alreadyExists)
        //        {
        //            // Get UNO from session
        //            var httpContext = _httpContextAccessor.HttpContext;
        //            var uno = httpContext?.Session.GetInt32("UNO");

        //            if (uno == null)
        //            {
        //                throw new InvalidOperationException("UNO not found in session.");
        //            }

        //            var actionTaken = new Ncactaken
        //            {
        //                Acid = model.ACID,
        //                Uno = uno,
        //                Showclient = model.ACShow
        //            };

        //            _context.Ncactakens.Add(actionTaken);
        //        }
        //    }

        //    if (model.UploadedFile != null)
        //    {
        //        var file = model.UploadedFile;

        //        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
        //        {
        //            throw new InvalidOperationException("Only PDF files are allowed.");
        //        }

        //        var oid = action?.Oid;
        //        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "Bocw");
        //        Directory.CreateDirectory(folderPath);

        //        string fileName = Path.GetFileName(file.FileName);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }

        //        var ncFile = new Ncfile
        //        {
        //            Oid = oid,
        //            Flink = action.Acid,
        //            Fname = fileName,
        //            Fupdate = DateOnly.FromDateTime(DateTime.Today)
        //        };

        //        _context.Ncfiles.Add(ncFile);
        //    }

        //    _context.SaveChanges();
        //}

        //public void UpdateOnlyNCACTION(BOCWEditViewModel model)                  // good h yeh wala
        //{
        //    var action = _context.Ncactions.FirstOrDefault(x => x.Acid == model.ACID);
        //    if (action != null)
        //    {
        //        action.Acdetail = model.ACDetail;
        //        action.Acshow = model.ACShow;
        //        action.Acremarks = model.ACRemarks;
        //        action.Acidate = model.ACIDate;
        //    }

        //    if (model.UploadedFile != null)
        //    {
        //        var file = model.UploadedFile;

        //        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
        //        {
        //            throw new InvalidOperationException("Only PDF files are allowed.");
        //        }

        //        var oid = action?.Oid;
        //        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "Bocw");
        //        Directory.CreateDirectory(folderPath);

        //        string fileName = Path.GetFileName(file.FileName);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }

        //        var ncFile = new Ncfile
        //        {
        //            Oid = oid,
        //            Flink = action.Acid,
        //            Fname = fileName,
        //            Fupdate = DateOnly.FromDateTime(DateTime.Today)
        //        };

        //        _context.Ncfiles.Add(ncFile);
        //    }

        //    _context.SaveChanges();
        //}

        //public void UpdateData(BOCWEditViewModel model)
        //{
        //    var bocw = _context.Ncbocws.FirstOrDefault(x => x.Lcode == model.LCode && x.TransactionId == model.TransactionID);
        //    var action = _context.Ncactions.FirstOrDefault(x => x.Acid == model.ACID);

        //    if (bocw != null)
        //    {
        //        bocw.DueDate = model.DueDate;
        //        bocw.Status = model.Status;
        //        bocw.CompletionDate = model.CompletionDate;
        //    }

        //    if (action != null)
        //    {
        //        action.Acdetail = model.ACDetail;
        //        action.Acshow = model.ACShow;
        //        action.Acremarks = model.ACRemarks;
        //        action.Acidate = model.ACIDate;
        //    }

        //    if (model.UploadedFile != null && action != null)
        //    {
        //        var file = model.UploadedFile;

        //        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
        //        {
        //            throw new InvalidOperationException("Only PDF files are allowed.");
        //        }

        //        var oid = action.Oid;
        //        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", oid.ToString(), "Bocw");
        //        Directory.CreateDirectory(folderPath);

        //        string fileName = Path.GetFileName(file.FileName);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }

        //        var ncFile = new Ncfile
        //        {
        //            Oid = oid,
        //            Flink = action.Acid,
        //            Fname = fileName,
        //            Fupdate = DateOnly.FromDateTime(DateTime.Today)
        //        };

        //        _context.Ncfiles.Add(ncFile);
        //    }

        //    _context.SaveChanges(); // MANDATORY
        //}
    }


}
