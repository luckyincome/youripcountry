using youripcountry.DAL.Base;
using Microsoft.AspNetCore.Mvc.Routing;
using youripcountry.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youripcountry.DAL
{
    public class GSgameService : IGSgameService
    {        

        public string getcountrycode(string ip)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ipaddress", ParameterValue = ip });
            return new SqlHelper().GetRecord<string>("pro_find_ip_country_byip", parameters);
        }


        public List<QMBlackListModel> getAllQMBlackList()
        {
            return new SqlHelper().GetRecords<QMBlackListModel>("pro_tb_qmBlackList_getAll", null);
        }
    }
}