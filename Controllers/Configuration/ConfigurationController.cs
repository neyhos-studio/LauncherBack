using LauncherBack.Controllers.Connexion;
using LauncherBack.Helpers;
using Microsoft.AspNetCore.Mvc;
using MSG = LauncherBack.Helpers.Messages;
using CONST = LauncherBack.Helpers.Constantes;
using CRED = LauncherBack.Helpers.Config.Credentials;
using System;

namespace LauncherBack.Controllers.Configuration
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        [HttpPost]
        [ActionName("Forbidden Words")]
        public ResponseFront AddForbiddenWord([FromBody] string forbiddenWord)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();

            ResponseFront responseFront = new ResponseFront();

            if (bdd.InsertForbiddenWord(forbiddenWord)){
                responseFront.hasError = true;
                responseFront.response = MSG.CONFIGURATION_MOT_INTERDIT_OK;
                return responseFront;
            } else
            {
                responseFront.hasError = true;
                responseFront.error = MSG.CONFIGURATION_MOT_INTERDIT_NOK;
                return responseFront;
            }
        }

        [HttpPost]
        [ActionName("Banishment")]
        public void Banishment([FromBody] RequestBannissement request)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();

            bdd.BanningUser(request.startDate, request.duration, request.reason, request.idAccount);

        }
    }
}
