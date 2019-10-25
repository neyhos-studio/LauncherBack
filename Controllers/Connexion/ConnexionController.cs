using LauncherBack.Controllers.Utilisateur;
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
        [ActionName("Connection")]
        public ResponseFront Connection([FromBody] RequestFrontConnexion request)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();
            Banishment banishment = new Banishment();

            string mdpCrypt = ShaHash.GetShaHash(request.password);
            string concatPasswordKeys = String.Concat(CONST.KEY_CRYPTAGE, mdpCrypt, CONST.KEY_CRYPTAGE);
            string stringCrypt = ShaHash.GetShaHash(concatPasswordKeys);

            request.password = stringCrypt;

            try
            {
                int exist = bdd.Connection(request);

                if(exist == 0)
                {
                    log.Info("Compte introuvable en BDD");
                    responseFront.hasError = true;
                    responseFront.error = MSG.COMPTE_INTROUVABLE;
                    return responseFront;
                }

                int idAccount = bdd.RetrieveIdAccount(request);

                banishment = bdd.TestIfUserBan(idAccount);

                banishment.endFormalize = banishment.end.ToString("dd.MM.yyyy-HH:mm:ss");

                if (banishment.hasBanned)
                {
                    log.Info(MSG.COMPTE_BANNI);
                    responseFront.hasError = true;
                    responseFront.error = banishment;
                    return responseFront;
                }
                
                string tokenClient = generationToken(CONST.TOKEN_SIZE);
                string tokenServer = generationToken(CONST.TOKEN_SIZE);

                bdd.InsertToken(idAccount, tokenServer, tokenClient);
                bdd.NowOnline(idAccount);

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
            string caracteres = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN123456789";
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
