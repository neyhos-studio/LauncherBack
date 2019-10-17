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

namespace LauncherBack.Controllers.Utilisateur
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class UtilisateurController : ControllerBase
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();
        Utilisateur utilisateur = new Utilisateur();

        [HttpPost]
        [ActionName("RetournerUtilisateur")]
        public ResponseFront Connexion([FromBody] RequestFrontUtilisateur request)
        {
            Bdd bdd = ConnexionBdd.connexionBase();
            responseFront.response = bdd.RecupUtilisateur(request);
            return responseFront;
        }

    }
}
