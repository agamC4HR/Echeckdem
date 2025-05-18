
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Handlers
{
    public interface IFilter
    {
        JsonResult GetLocationsByOid(string oid);

        JsonResult GetCityByOid(string oid);

        JsonResult GetStateByOid(string oid);
        JsonResult GetLocations();

        JsonResult GetCity();

        JsonResult GetState();
    }
}
