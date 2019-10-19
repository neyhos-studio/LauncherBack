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

        [HttpPost]
        [ActionName("RetournerUtilisateur")]
        public ResponseFront Connexion([FromBody] RequestFrontUtilisateur request)
        {
            Bdd bdd = ConnexionBdd.connexionBase();
            User utilisateur = new User();

            int idAccount = bdd.RecupIdUtilisateur(request);

            utilisateur = bdd.RecupUtilisateur(idAccount);           

            if(utilisateur.pseudo == null)
            {
                responseFront.hasError = true;
                responseFront.error = "Pas d'utilisateur correspond à se token";
                return responseFront;
            }

            log.Info("Récupération des informations du joueur " + utilisateur.pseudo);
            responseFront.response = utilisateur;
            return responseFront;
        }

    }
}
