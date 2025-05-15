using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly DbEcheckContext _context;

        public AdminController(DbEcheckContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var level = HttpContext.Session.GetInt32("User Level");
            return level.HasValue && level.Value == 1;
        }


        [HttpGet("tsql-kar")]
        public IActionResult QueryTool()
        {
            if (!IsAdmin())
                //return RedirectToAction("AccessDenied", "Home");
                return Redirect("https://comply4hr.com/");


            return View();
        }

        [HttpPost("run-query")]
        public async Task<IActionResult> RunQuery([FromBody] SqlQueryInput input)
        {
            if (!IsAdmin())
                return Unauthorized("Access denied. Admins only.");

            if (string.IsNullOrWhiteSpace(input.Query) ||
                !input.Query.Trim().ToLower().StartsWith("select"))
            {
                return BadRequest("Only SELECT queries are allowed.");
            }

            var results = new List<Dictionary<string, object>>();

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = input.Query;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            results.Add(row);
                        }
                    }
                }
            }

            return Json(results);
        }
        //[HttpPost("run-query")]
        //public async Task<IActionResult> RunQuery([FromBody] SqlQueryInput input)
        //{
        //    var results = new List<Dictionary<string, object>>();

        //    if (string.IsNullOrWhiteSpace(input.Query) ||
        //        !input.Query.Trim().ToLower().StartsWith("select"))
        //    {
        //        return BadRequest("Only SELECT queries are allowed.");
        //    }

        //    using (var conn = _context.Database.GetDbConnection())
        //    {
        //        await conn.OpenAsync();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = input.Query;
        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var row = new Dictionary<string, object>();
        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
        //                    results.Add(row);
        //                }
        //            }
        //        }
        //    }

        //    return Json(results);
        //}

    }
}
