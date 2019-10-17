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
        [ActionName("Inscription")]
        public ResponseFront AjouterMotInterdit([FromBody] string motInterdit)
        {
            Bdd bdd = ConnexionBdd.connexionBase();

            ResponseFront responseFront = new ResponseFront();

            if (bdd.InsertMotInterdit(motInterdit)){
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
    }
}
