using youripcountry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace youripcountry.DAL
{
   public interface IGSgameService
    {
        List<QMBlackListModel> getAllQMBlackList();


        string getcountrycode(string ip);

    }
}
 