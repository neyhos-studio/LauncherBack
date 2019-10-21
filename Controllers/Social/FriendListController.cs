using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Helpers;
using log4net;
using Microsoft.AspNetCore.Mvc;
using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;

namespace LauncherBack.Controllers.Social
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendListController : ControllerBase
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("RetrieveFriendList")]
        public static List<Friend> RetrieveFriendList([FromBody] int idAccount)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();
            List<Friend> friendList = new List<Friend>();

            friendList = bdd.RetrieveFriendListDatabase(idAccount);
            log.Debug(friendList);

            return friendList;
        }

    }
}
