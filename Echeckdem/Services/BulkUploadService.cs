using ClosedXML.Excel;
using Echeckdem.Models;




namespace Echeckdem.Services
{
    public class BulkUploadService : IBulkUploadService
    {
        private readonly DbEcheckContext _context;

        public BulkUploadService(DbEcheckContext context)
        {
            _context = context;
        }

        public async Task<int> UploadLocationDataAsync(IFormFile file )         // addded here 
        {
            var locations = new List<Ncmloc>();

            if (file.Length > 0)
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
                            var location = new Ncmloc
                            {
                                Lcode = row.Cell(1).GetValue<string>().Substring(0, Math.Min(15, row.Cell(1).GetValue<string>().Length)),
                                 Oid = row.Cell(2).GetValue<string>().Substring(0, Math.Min(30, row.Cell(2).GetValue<string>().Length)),
                                //Oid = oid,
                                Lname = row.Cell(3).GetValue<string>().Substring(0, Math.Min(100, row.Cell(3).GetValue<string>().Length)),
                                Lcity = row.Cell(4).GetValue<string>().Substring(0, Math.Min(30, row.Cell(4).GetValue<string>().Length)),
                                Lstate = row.Cell(5).GetValue<string>().Substring(0, Math.Min(30, row.Cell(5).GetValue<string>().Length)),
                                Lregion = row.Cell(6).GetValue<string>().Substring(0, Math.Min(30, row.Cell(6).GetValue<string>().Length))

                            };

                            locations.Add(location);

                        }
                    }
                    await _context.Ncmlocs.AddRangeAsync(locations);
                    await _context.SaveChangesAsync();
                    

                }

                return locations.Count;
            }

            return 0;
        }
    }
}