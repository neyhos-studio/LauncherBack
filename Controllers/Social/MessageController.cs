using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Helpers;
using log4net;
using Microsoft.AspNetCore.Mvc;


namespace LauncherBack.Controllers.Social.Tchat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("SendMessage")]
        public static void SendMessage([FromBody] Message requestFront)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();

            //bdd.SendMessage(requestFront);

        }
    }
}
