using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Presentation;
using Echeckdem.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Echeckdem.Services
{
    public class ProjectCalendarGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;
        private readonly DbEcheckContext _context;
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ProjectCode { get; set; }

        public string lcode { get; set; }
        public string lname { get; set; }


        public ProjectCalendarGenerator(DateTime projectStartDate, DateTime projectEndDate, string projectCode, string Lcode, string Lname, IConfiguration configuration, DbEcheckContext context)
        {
            ProjectStartDate = projectStartDate;
            ProjectEndDate = projectEndDate;
            ProjectCode = projectCode;
            lcode = Lcode;
            lname = Lname;
            _configuration = configuration;
            sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionStrings"));
            _context = context;
        }
        public async Task<string> insertquery(string scopeid, DateTime duedate, int status, string task)
        {

            string query = $"INSERT INTO dbo.Ncbocw (ScopeId, ProjectCode, Lcode, Lname, DueDate, Status, Task,CreateDate) " +
                $"VALUES ('{scopeid}', '{ProjectCode}', '{lcode}', '{lname}', '{duedate.ToString("yyyy-MM-dd")}', {status}, '{task}',GetDate())";

            //transaction log
            string logquery = "insert into ncaction(acid, oid, aclink, lcode, actp, actitle, acshow, acstatus, acidate, acruser, acrdate)"+$"values ()";

            sqlConnection.Open();
            SqlCommand comm = new SqlCommand(query, sqlConnection);
            await comm.ExecuteNonQueryAsync();
            sqlConnection.Close();
            return $"{task} added";
        }
        public async Task<string> GenerateOneTimeDueDate(int offset, string reference, string scopeid)
        {
            DateTime duedate = reference == "Start"
                ? ProjectStartDate.AddDays(offset)
                : ProjectEndDate.AddDays(offset);
            var scopename = _context.BocwScopes.Where(x => x.ScopeId == scopeid).Select(x => x.ScopeName).FirstOrDefault();
            int status = duedate <= DateTime.Now ? 0 : 2;
            if (scopename != null)
                return await insertquery(scopeid, duedate, status, scopename);
            else return $"Scope Name not found for {scopeid}";
        }

        public async Task<string> GenerateMonthlyVendorAuditWindows(string scopeid)
        {
            string retstring = string.Empty;
            try
            {
                
                DateTime auditMonth = new DateTime(ProjectStartDate.Year, ProjectStartDate.Month, 1);
                DateTime endMonth = new DateTime(ProjectEndDate.Year, ProjectEndDate.Month, 1);
                while (auditMonth <= endMonth)
                {
                    DateTime duedate = new DateTime(auditMonth.Year, auditMonth.Month, 1).AddMonths(2).AddDays(9);
                    int status = duedate <= DateTime.Now ? 0 : 2;
                    string inserting = await insertquery(scopeid, duedate, status, $"Vendor Audit for {auditMonth.ToString("MMMM")},{auditMonth.Year.ToString()}");
                   // Console.WriteLine(inserting);
                    retstring = retstring + "\n" + inserting;

                    auditMonth = auditMonth.AddMonths(1);
                }
                return retstring;
            }
            catch (Exception ex)
            {
                return retstring = retstring + "\n" + $"Error in GenerateMonthlyVendorAuditWindows: {ex.Message}";
            }

        }

        public async Task<string> GenerateCalendarYearEndDueDates(string scopeid)
        {
            string retstring = string.Empty;
            int startyear = ProjectStartDate.Year;
            int endyear = ProjectEndDate.Year;
            int monthdiff = ((ProjectEndDate.Year- ProjectStartDate.Year)*12)+ProjectEndDate.Month-ProjectStartDate.Month;
            if (startyear == endyear)
            {
                DateTime duedate=GetCappedFinalDueDate(ProjectEndDate);
                int status = duedate <= DateTime.Now ? 0 : 2;
                var scopename = _context.BocwScopes.Where(x => x.ScopeId == scopeid).Select(x => x.ScopeName).FirstOrDefault();
                retstring=await insertquery(scopeid, duedate, status, $"{scopename} For Project Ending {startyear}");
                return retstring;
            }

            if (startyear < endyear+1 && (monthdiff>=12 || monthdiff<12))
            { 
                DateTime firstduedate = new DateTime(endyear, 1, 31);
                int status = firstduedate <= DateTime.Now ? 2 : 0;
                var scopename = _context.BocwScopes.Where(x => x.ScopeId == scopeid).Select(x => x.ScopeName).FirstOrDefault();
                retstring = await insertquery(scopeid, firstduedate, status, $"{scopename} For CY Ending {startyear}");
                DateTime finalduedate = GetCappedFinalDueDate(ProjectEndDate);
                status = firstduedate <= DateTime.Now ? 2 : 0;
                retstring = retstring + "\n" + await insertquery(scopeid, finalduedate, status, $"{scopename} For Project Ending {ProjectEndDate.ToShortDateString()}");
                return retstring;
            }
            if (endyear-startyear>=2)
            {
                var scopename = _context.BocwScopes.Where(x => x.ScopeId == scopeid).Select(x => x.ScopeName).FirstOrDefault();
                for (int year = ProjectStartDate.Year; year < ProjectEndDate.Year; year++)
                {
                    DateTime duedate = new DateTime(year + 1, 1, 31);
                    if (duedate > ProjectStartDate && duedate < ProjectEndDate)
                    {
                        int status = duedate <= DateTime.Now ? 2 : 0;
                        retstring = retstring + "\n" + await insertquery(scopeid, duedate, status, $"{scopename} For CY Ending {year}");
                    }
                }
                DateTime finalduedate = ProjectEndDate.AddMonths(1);
                int statuss = finalduedate <= DateTime.Now ? 2 : 0;
                retstring = retstring + "\n" + await insertquery(scopeid, finalduedate, statuss, $"{scopename} For Project Ending {ProjectEndDate.ToShortDateString()}");
                return retstring;
            }


            return $"No condition matched in GenerateCalendarYearEndDueDates with Project tenure {monthdiff} and Project Start year {startyear} and End year {endyear}";

        }
        private DateTime GetCappedFinalDueDate(DateTime projectEnd)
        {
            DateTime tentative = projectEnd.AddMonths(1);
            int cappedMonth = Math.Min(12, tentative.Month);
            int cappedDay = Math.Min(projectEnd.Day, DateTime.DaysInMonth(projectEnd.Year, cappedMonth));
            return new DateTime(projectEnd.Year, cappedMonth, cappedDay);

        }
        public async Task<string> GenerateHalfYearlyDueDates(string scopeid) 
        {
            string retstring = string.Empty;
            DateTime current = new DateTime(ProjectStartDate.Year, 1, 1);
            if (ProjectStartDate.Month > 6)
                current = new DateTime(ProjectStartDate.Year, 7, 1);
            int status = 2;
            while (current < ProjectEndDate)
            {
                DateTime duedate;
                string task;

                if (current.Month == 1) // Jan–Jun → due 31 July
                {
                    duedate = new DateTime(current.Year, 7, 31);
                    task= $"H1 {current.Year} CLRA License Return Filing";
                 }
                else // Jul–Dec → due 31 Jan of next year
                {
                    duedate = new DateTime(current.Year + 1, 1, 31);
                    task = $"H2 {current.Year} CLRA License Return Filing";
                }

                if (duedate > ProjectStartDate && duedate < ProjectEndDate)
                {
                    status = duedate <= DateTime.Now ? 0 : 2;
                    retstring = retstring + "\n" + await insertquery(scopeid, duedate, status, task);
                }

                current = current.AddMonths(6);
            }

            DateTime finalduedate = ProjectEndDate.AddMonths(1);
            status = finalduedate <= DateTime.Now ? 0 : 2;
            retstring = retstring + "\n" + await insertquery(scopeid, finalduedate, status, $"CLRA License return for Project Ending on {ProjectEndDate.ToShortDateString()}");




            return retstring;
        }
    }
}
