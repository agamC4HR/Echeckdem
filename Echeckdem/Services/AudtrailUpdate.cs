using Azure.Core;
using DocumentFormat.OpenXml.Presentation;
using Echeckdem.Handlers;
using Echeckdem.Models;

namespace Echeckdem.Services
{
    public class AudtrailInput
    {


        public string? Uids { get; set; }

        public string? Origin { get; set; }

        public string? Activity { get; set; }

        public string? Ttable { get; set; }

        public string? Details { get; set; }

        public DateTime? Tdate { get; set; }

        public DateTime? Ttime { get; set; }

        public string? UserAgent { get; set; }

        public string? RequestPath { get; set; }
        public string? Country { get; set; }

        public string? City { get; set; }
        public string? SessionID { get; set; }

    }


    public class AudtrailUpdate : IAudtrail
    {
        private readonly DbEcheckContext _context;
        private readonly HttpContext _httpContext;
        private class IpApiResponse
        {
            public string? Country { get; set; }
            public string? City { get; set; }
        }
        public AudtrailUpdate(DbEcheckContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;
            
        }
        public async Task<bool> AddAuditTrailAsync(string userId, string tableName, string details)
        {
            var origin = GetUserIp(); 
            var userAgent = _httpContext.Request.Headers["User-Agent"].ToString();
            var requestPath = _httpContext.Request.Path.ToString();
            var sessionId = _httpContext.Session?.Id; 

            var (country, city) = await GetGeoLocationAsync(origin);
            var auditTrail = new Audtrail
            {
                 Uids= userId,
                Origin =origin,
                Ttable = tableName,
                Details = details,
                Tdate = DateTime.UtcNow,
                UserAgent = userAgent,
                RequestPath = requestPath,
                SessionID = sessionId,
                Country = country,
                City = city

            };
            await _context.Audtrails.AddAsync(auditTrail);
            await _context.SaveChangesAsync();
            return true;
        }
        public string GetUserIp()
        {
            var ipAddress = _httpContext.Connection.RemoteIpAddress?.ToString();

            if (_httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                var forwardedFor = _httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    ipAddress = forwardedFor.Split(',')[0];
                }
            }

            return ipAddress;
        }
        private async Task<(string? Country, string? City)> GetGeoLocationAsync(string ipAddress)
        {
            using var client = new HttpClient();
            var url = $"http://ip-api.com/json/{ipAddress}";
            var response = await client.GetFromJsonAsync<IpApiResponse>(url);
            return (response?.Country, response?.City);
        }

    }


}
