using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Utilisateur;
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
        [ActionName("RetournerFriendList")]
        public ResponseFront RetournerFriendList([FromBody] int idAccount)
        {
            Bdd bdd = ConnexionBdd.connexionBase();
            List<User> friendList = new List<User>();

            friendList = bdd.RecupFriendList(idAccount);

            responseFront.response = friendList;

            return responseFront;
        }

    }
}
