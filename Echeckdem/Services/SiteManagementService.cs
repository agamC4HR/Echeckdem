using DocumentFormat.OpenXml.Office2013.Excel;
using Echeckdem.Models;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;


namespace Echeckdem.Services
{
    public class SiteManagementService : ISiteManagementService
    {
        private readonly DbEcheckContext _dbEcheckContext;
        
        public SiteManagementService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public async Task<List<OrganisationsListViewModel>> GetActiveOrganizationsAsync()
        {
            return await _dbEcheckContext.Ncmorgs
                .Where(o => o.Oactive == 1)
                .Select(o => new OrganisationsListViewModel
                {
                    oid = o.Oid,
                    Oname = o.Oname
                })
                .ToListAsync();
        }

        public async Task<List<LocationViewModel>> GetLocationsByOidAsync(string oid)
        {
            return await _dbEcheckContext.Ncmlocs
                .Where(l => l.Oid == oid)
                .Select(l => new LocationViewModel
                {
                    Lcode = l.Lcode,
                    Lname = l.Lname,
                    Lstate = l.Lstate,
                    Ltype = l.Ltype,
                    Iscloc = l.Iscloc,
                    Iscentral = l.Iscentral,
                    Lregion = l.Lregion,
                    Laddress = l.Laddress,
                    Lcontact = l.Lcontact,
                    Lconno = l.Lconno,
                    Lconemail = l.Lconemail,
                    Cemail = l.Cemail,
                    Iemail = l.Iemail
                })
                .ToListAsync();


        }

        //public async Task<LocationViewModel?> GetLocationByLcodeAsync(string lcode)
        //{
        //    return await _dbEcheckContext.Ncmlocs
        //        .Where(l => l.Lcode == lcode)
        //        .Select(l => new LocationViewModel
        //        {
        //            Oid = l.Oid,
        //            Lcode = l.Lcode,
        //            Lname = l.Lname,
        //            Lstate = l.Lstate,
        //            Ltype = l.Ltype,
        //            Iscentral = l.Iscentral,
        //            Iscloc = l.Iscloc,
        //            Lregion = l.Lregion,
        //            Laddress = l.Laddress,
        //            Lcontact = l.Lcontact,
        //            Lconno = l.Lconno,
        //            Lconemail = l.Lconemail,
        //            Cemail = l.Cemail,
        //            Iemail = l.Iemail
        //        })
        //        .FirstOrDefaultAsync();
        //}

        //public async Task UpdateLocationAsync(LocationViewModel model)
        //{
        //    var loc = await _dbEcheckContext.Ncmlocs.FirstOrDefaultAsync(l => l.Lcode == model.Lcode);
        //    if (loc != null)
        //    {
        //        loc.Lname = model.Lname;
        //        loc.Lstate = model.Lstate;
        //        loc.Ltype = model.Ltype;
        //        loc.Iscentral = model.Iscentral;
        //        loc.Iscloc = model.Iscloc;
        //        loc.Lregion = model.Lregion;
        //        loc.Laddress = model.Laddress;
        //        loc.Lcontact = model.Lcontact;
        //        loc.Lconno = model.Lconno;
        //        loc.Lconemail = model.Lconemail;
        //        loc.Cemail = model.Cemail;
        //        loc.Iemail = model.Iemail;

        //        await _dbEcheckContext.SaveChangesAsync();
        //    }
        //}

    }
}
