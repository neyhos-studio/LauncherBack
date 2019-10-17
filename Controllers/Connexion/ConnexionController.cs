using LauncherBack.Helpers;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;

namespace LauncherBack.Controllers.Connexion
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ConnexionController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("Connexion")]
        public ResponseFront Connexion([FromBody] RequestFrontConnexion request)
        {
            Bdd bdd = ConnexionBdd.connexionBase(); 

            string mdpCrypt = ShaHash.GetShaHash(request.password);
            string concatPasswordKeys = String.Concat(CONST.KEY_CRYPTAGE, mdpCrypt, CONST.KEY_CRYPTAGE);
            string stringCrypt = ShaHash.GetShaHash(concatPasswordKeys);

            request.password = stringCrypt;

            try
            {
                int exist = bdd.Connexion(request);

                if(exist == 0)
                {
                    log.Info("Compte introuvable en BDD");
                    responseFront.hasError = true;
                    responseFront.error = MSG.COMPTE_INTROUVABLE;
                    return responseFront;
                }

                int idAccount = bdd.RecupIdAccount(request);
                string tokenClient = generationToken(CONST.LONGUEUR_TOKEN);
                string tokenServer = generationToken(CONST.LONGUEUR_TOKEN);

                bdd.InsertToken(idAccount, tokenServer, tokenClient);
                bdd.PassageEnLigne(idAccount);

                log.Info("Utilisateur #" + idAccount + " vient de connecter");

                responseFront.response = tokenClient;
                return responseFront;
            }
            catch(Exception e)
            {
                log.Error("Erreur durant la connexion", e);
                responseFront.hasError = true;
                responseFront.error = MSG.CONNEXION_IMPOSSIBLE;
                return responseFront;
            }
        }

        private string generationToken(int longueurToken)
        {
            string caracteres = "azertyuiopqsdfghjklmwxcvbn123456789";
            Random tokenAlea = new Random();
            string token = "";

            for (int i = 0; i < longueurToken; i++) // 254 caracteres
            {
                int majOrMin = tokenAlea.Next(2);
                string carac = caracteres[tokenAlea.Next(0, caracteres.Length)].ToString();
                if (majOrMin == 0)
                {
                    token += carac.ToUpper(); // Maj
                }
                else
                {
                    token += carac.ToLower(); //Min
                }
            }

            return token;
        }

        

    }

    
}
