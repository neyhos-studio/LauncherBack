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

namespace LauncherBack.Controllers.Games
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("RetrieveGameList")]
        public List<Game> RetrieveGameList()
        {
            Bdd bdd = DataBaseConnection.databaseConnection();
            List<Game> gameList = new List<Game>();

            gameList = bdd.RetrieveGameList();

            return gameList;
        }
    }
}
