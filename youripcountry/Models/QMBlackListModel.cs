using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace youripcountry.Models
{

    public class CurrentLocationModel
    {
        public List<QMBlackListModel> qmBlackList { get; set; }

        public QMBlackListModel currentLocation { get; set; }
        public string requestCountryCode { get; set; }
        public string requestIpAddress { get; set; }

        public string ipLookupKey { get; set; }

        public string countrycode { get; set; }
    }
    public class QMBlackListModel
    {
        public int id { get; set; }

        public string countryName { get; set; }

        public string countryName_mm { get; set; }

        public string countryName_zh { get; set; }

        public string countryName_th { get; set; }

        public string countryCode2Digit { get; set; }

        public string countryCode3Digit { get; set; }

        public string currentipAddress { get; set; }

        public string currentLocation {get ;set;}

        public string currentLocation_mm { get; set; }

        public string currentLocation_th { get; set; }

        public string currentLocation_zh { get; set; }

        public string currentCountryCode { get; set; }

    }
    public class CountryCodeModel
    {
        public string countryCode { get; set; }
    }
}
