using Microsoft.AspNetCore.Mvc;
using youripcountry.DAL;
using youripcountry.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace youripcountry.Controllers
{
    [ApiController]  
   
    [Route("api/country")]
    public class CountryController : ControllerBase
    {

        private readonly IGSgameService _gsGameService;
        private readonly IConfiguration _config;

        public CountryController(IGSgameService gsGameService, IConfiguration config)
        {
            this._config = config;
            this._gsGameService = gsGameService;
        }

        
       
        [HttpGet]
        [Route("getCountryCode")]
        public CurrentLocationModel getCountryCode() /*get country code - from mobile*/
        {
            CurrentLocationModel objcurrentLocation = new CurrentLocationModel();
            IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string resultIp = "";
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                resultIp = remoteIpAddress.ToString();

                //search on db with resultIp                
                objcurrentLocation.qmBlackList = _gsGameService.getAllQMBlackList();
                objcurrentLocation.ipLookupKey = _config.GetValue<string>("CountryCode:ipLookupKey");
                objcurrentLocation.countrycode = _gsGameService.getcountrycode(resultIp);
                return objcurrentLocation;


                //string url = "https://ipapi.co/" + resultIp + "/country/";
                //return Redirect(url);
            }
            return objcurrentLocation = new CurrentLocationModel();
        }
    }
}
