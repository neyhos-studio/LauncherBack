using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Social;
using LauncherBack.Helpers;
using log4net;
using Microsoft.AspNetCore.Mvc;
using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;

namespace LauncherBack.Controllers.Utilisateur
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class UtilisateurController : ControllerBase
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();
        Bdd bdd = DataBaseConnection.databaseConnection();

        [HttpPost]
        [ActionName("RetrieveUser")]
        public ResponseFront Connexion([FromBody] RequestFrontUtilisateur request)
        {
            
            User user = new User();

            int idAccount = bdd.RetrieveUserId(request);

            user = bdd.RetrieveUser(idAccount);
            user.friendList = FriendListController.RetrieveFriendList(idAccount);
            user.gameList = bdd.RetrieveUserGameList(idAccount);

            

            if(user.nickname == null)
            {
                responseFront.hasError = true;
                responseFront.error = "Pas d'utilisateur correspond à se token";
                return responseFront;
            }

            log.Info("Récupération des informations du joueur " + user.nickname);
            responseFront.response = user;
            return responseFront;
        }

        [HttpPost]
        [ActionName("Disconnection")]
        public Boolean Disconnection([FromBody] RequestFrontUtilisateur request)
        {
            int idAccount = bdd.RetrieveUserId(request);

            return bdd.disconectionUser(idAccount);
        }

    }
}
