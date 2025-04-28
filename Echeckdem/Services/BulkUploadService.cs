using ClosedXML.Excel;
using Echeckdem.Models;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace Echeckdem.Services
{
    public class BulkUploadService : IBulkUploadService
    {
        private readonly DbEcheckContext _context;

        public BulkUploadService(DbEcheckContext context)
        {
            _context = context;
        }
        async Task<string> GetStateAbbreviation(string fullStateName)                                              // function to map state to statemaster
        {
            var fullStateNameTrimmed = fullStateName.Trim();
            var state = await _context.Maststates.FirstOrDefaultAsync(s => s.Statedesc.ToLower() == fullStateNameTrimmed.ToLower());
            if (state == null)
            {
              Console.WriteLine($"State not found: {fullStateNameTrimmed}");
            }
            return state?.Stateid?? string.Empty;
        }


        public async Task<int> UploadLocationDataAsync(IFormFile file, string oid)         // bulk upload
        {
            var locations = new List<Ncmloc>();



            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file. Please upload a valid Excel file.");
            }



            try//if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1);  // skiping the first row heading row

                        foreach (var row in rows)
                        {

                            try
                            {
                                if (string.IsNullOrWhiteSpace(row.Cell(1).GetValue<string>()))
                                    throw new Exception("Site Name is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(3).GetValue<string>()))
                                    throw new Exception("State Name is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(5).GetValue<string>()))
                                    throw new Exception("Site Act is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(6).GetValue<string>()))
                                    throw new Exception("Site Address is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(7).GetValue<string>()))
                                    throw new Exception("Site FM is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(8).GetValue<string>()))
                                    throw new Exception("Site FM Email is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(9).GetValue<string>()))
                                    throw new Exception("Site FM Contact Number is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(10).GetValue<string>()))
                                    throw new Exception("Site Escalation1 is required.");

                                if (string.IsNullOrWhiteSpace(row.Cell(13).GetValue<string>()))
                                    throw new Exception("Site Setup Year is required.");
                                
                                var fullStateName = row.Cell(3).GetValue<string>().Substring(0, Math.Min(30, row.Cell(3).GetValue<string>().Length));
                                var stateAbbreviation = await GetStateAbbreviation(fullStateName);


                                if (string.IsNullOrEmpty(stateAbbreviation))
                                {
                                    throw new Exception($"Invalid State Name: {fullStateName}");
                                }


                                int? ParseYesNo(string value) =>
                            value?.Trim().ToLower() switch
                            {
                                "yes" => 1,
                                "no" => 0,
                                _ => null // Handle invalid values as null
                            };

                                var location = new Ncmloc
                                {
                                    Lcode = Guid.NewGuid().ToString().Substring(0, 8),
                                    Oid = oid,
                                    Lname = row.Cell(1).GetValue<string>().Substring(0, Math.Min(100, row.Cell(1).GetValue<string>().Length)),
                                    Lcity = row.Cell(2).GetValue<string>().Substring(0, Math.Min(30, row.Cell(2).GetValue<string>().Length)),
                                    Lstate = stateAbbreviation,
                                    Lregion = row.Cell(4).GetValue<string>().Substring(0, Math.Min(30, row.Cell(4).GetValue<string>().Length)),
                                    Ltype = row.Cell(5).GetValue<string>().Substring(0, Math.Min(30, row.Cell(5).GetValue<string>().Length)),
                                    Laddress = row.Cell(6).GetValue<string>().Substring(0, Math.Min(200, row.Cell(6).GetValue<string>().Length)),
                                    Lcontact = row.Cell(7).GetValue<string>().Substring(0, Math.Min(100, row.Cell(7).GetValue<string>().Length)),
                                    Lconemail = row.Cell(8).GetValue<string>().Substring(0, Math.Min(100, row.Cell(8).GetValue<string>().Length)),
                                    Lconno = row.Cell(9).GetValue<string>().Substring(0, Math.Min(50, row.Cell(9).GetValue<string>().Length)),
                                    Cemail = row.Cell(10).GetValue<string>().Substring(0, Math.Min(100, row.Cell(10).GetValue<string>().Length)),
                                    Iscentral = ParseYesNo(row.Cell(11).GetValue<string>()), // Yes - 1 No - 0
                                    Iscloc = ParseYesNo(row.Cell(12).GetValue<string>()),
                                    Lsetup = row.Cell(13).GetValue<int?>(),
                                    Lactive = ParseYesNo(row.Cell(14).GetValue<string>()),
                                    Ispf = ParseYesNo(row.Cell(15).GetValue<string>()),
                                    Isesi = ParseYesNo(row.Cell(16).GetValue<string>())
                                };
                                locations.Add(location);
                            }

                            catch (Exception rowEx)
                            {
                                throw new Exception($"Error in row {row.RowNumber()} - {rowEx.Message}");
                            }
                        }

                    }
                    await _context.Ncmlocs.AddRangeAsync(locations);
                    await _context.SaveChangesAsync();
                }

                return locations.Count;
            }

            catch (Exception ex)
            {
                throw new Exception($"Upload failed: {ex.Message}");
            }
            //return 0;
        }
    }
}   