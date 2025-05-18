using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.Handlers;
using Echeckdem.Models;
using Echeckdem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
namespace Echeckdem.Services
{
    public class Filter:IFilter
    {
        private readonly DbEcheckContext _context;
        private readonly HttpContext _httpcontext;
        public Filter(DbEcheckContext context, IHttpContextAccessor _httpContextAccessor)
        {
            _context = context;
            _httpcontext = _httpContextAccessor.HttpContext;
        }
        public JsonResult GetLocationsByOid(string oid)
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var locations = _UserlocationList.Where(x => x.Oid == oid && !string.IsNullOrEmpty(x.Site)).Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
           
            return new JsonResult(locations);
        }
        public JsonResult GetCityByOid(string oid)
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var cities= _UserlocationList.Where(x => !string.IsNullOrEmpty(x.Lcity) && x.Oid==oid).Select(x => new SelectListItem { Value = x.Lcity, Text = x.Lcity }).DistinctBy(x => x.Value).ToList();
            return new JsonResult(cities);
        }
        public JsonResult GetStateByOid(string oid)
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var states = _UserlocationList.Where(x => x.Oid == oid  && !string.IsNullOrEmpty(x.Lstate)).Select(x => new SelectListItem { Value = x.Lstate, Text = x.Lstate }).DistinctBy(x => x.Value).ToList();
            return new JsonResult(states);
        }
        public JsonResult GetLocations()
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var locations = _UserlocationList.Where(x => !string.IsNullOrEmpty(x.Site)).Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
            return new JsonResult(locations);
        }
        public JsonResult GetCity()
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var cities = _UserlocationList.Where(x => !string.IsNullOrEmpty(x.Lcity)).Select(x => new SelectListItem { Value = x.Lcity, Text = x.Lcity }).DistinctBy(x => x.Value).ToList();
            return new JsonResult(cities);
        }
        public JsonResult GetState()
        {
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(_httpcontext.Session.GetString("Userlocation"));
            var states = _UserlocationList.Where(x => !string.IsNullOrEmpty(x.Lstate)).Select(x => new SelectListItem { Value = x.Lstate, Text = x.Lstate }).DistinctBy(x => x.Value).ToList();
            return new JsonResult(states);
        }
    }
}
