using LauncherBack.Controllers.Connexion;
using LauncherBack.Helpers;
using Microsoft.AspNetCore.Mvc;
using MSG = LauncherBack.Helpers.Messages;
using CONST = LauncherBack.Helpers.Constantes;
using CRED = LauncherBack.Helpers.Config.Credentials;
using System;
using NHibernate;

namespace LauncherBack.Controllers.Configuration
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        /*[HttpPost]
        [ActionName("Forbidden Words")]
        public ResponseFront AddForbiddenWord([FromBody] ForbiddenWord forbiddenWord)
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
        }*/

        [HttpPost]
        [ActionName("Forbidden Words")]
        public void AddForbiddenWord([FromBody] ForbiddenWord forbiddenWord)
        {
            ISession session = NhibernateSession.OpenSession();

            ForbiddenWordHib forbiddenWordHib = new ForbiddenWordHib();

            forbiddenWordHib.word = forbiddenWord.word;
            session.Save(forbiddenWordHib);
            session.Close();
        }


        [HttpPost]
        [ActionName("Banishment")]
        public void Banishment([FromBody] RequestBannissement request)
        {
            Bdd bdd = DataBaseConnection.databaseConnection();

            bdd.BanningUser(request.startDate, request.duration, request.reason, request.idAccount);
        }

        [HttpPost]
        [ActionName("Clean Database")]
        public string CleanDatabase([FromBody] int nbr)
        {
            if (CONST.nbrSecret == nbr)
            {
                Bdd bdd = DataBaseConnection.databaseConnection();
                bdd.cleanDatabase();

                return "Database clean !";
            }else
            {
                return "T'es pas à ta place ici gros naze !";
            }
            
        }

        [HttpPost]
        [ActionName("Clean tokens")]
        public string CleanTokens([FromBody] int nbr)
        {
            if(CONST.nbrSecret == nbr){
                Bdd bdd = DataBaseConnection.databaseConnection();
                bdd.cleanTokens();
                return "Table tokens clean !";
            }else
            {
                return "T'es pas à ta place ici gros naze !";
            }
        }

        [HttpPost]
        [ActionName("Disconnect All Users")]
        public string DisconnectAllUsers([FromBody] int nbr)
        {
            if(CONST.nbrSecret == nbr)
            {
                Bdd bdd = DataBaseConnection.databaseConnection();
                bdd.disconnectAllUsers();
                return "All users have been logged out";
            }else
            {
                return "Tu n'es pas à ta place ici gros naze !";
            }
        }
    }
}
