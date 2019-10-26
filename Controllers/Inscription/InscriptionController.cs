using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;
using CRED = LauncherBack.Helpers.Config.Credentials;
using LauncherBack.Helpers;
using LauncherBack.Controllers.Connexion;
using log4net;
using System.Reflection;

namespace LauncherBack.Controllers.Inscription
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InscriptionController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("Registration")]
        public ResponseFront Inscription([FromBody] RequestFrontInscription request)
        {

            Bdd bdd = DataBaseConnection.databaseConnection();


            #region Critères d'acceptances
            //On test les critères d'acceptances de l'Email / Password / Pseudo

            //Test si l'adresse Email existe ou non
            if (bdd.TestIfEmailExist(request.email))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_EMAIL_EXIST;
                return responseFront;
            }

            //Test de la taille de MDP
            if (request.password.Length < CONST.MINIMUM_PASSWORD_LENGTH)
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PASSWORD_COURT;
                return responseFront;
            }

            //Test de la taille du PSEUDO
            if (request.nickname.Length > CONST.MAXIMUM_NICKNAME_LENGTH)
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_LONG;
                return responseFront;
            }

            //Test du contenu du PSEUDO (mots interdis)
            //string[] listeMotsInterdis = CONST.MOTS_INTERDIS.Split('|');
            bool test = false;
            List<String> listeMotsInterdits = bdd.RetrieveListOfForbiddenWord();

            for (int i = 0; i < listeMotsInterdits.Count(); i++)
            {
                string motInterditNormaliser = listeMotsInterdits[i].ToString().ToLower();
                string pseudoNormaliser = request.nickname.ToLower();
                if (pseudoNormaliser.Contains(motInterditNormaliser))
                {
                    test = true;
                }
            }

            if (test)
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_MOTS_INTERDIT;
                return responseFront;
            }

            //Test de la validation d'une adresse Email
            if (!request.email.Contains("@"))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_EMAIL_INVALID;
                return responseFront;
            }

            //Test si le PSEUDO contient plus de 2 espaces
            if (request.nickname.Contains("  "))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_2_ESPACES;
                return responseFront;
            }

            //Test si le PSEUDO contient plus de 2 -
            if (request.nickname.Contains("--"))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_2_TIRETS;
                return responseFront;
            }
            #endregion

            #region Cryptage du MDP et de l'Email
            string mdpCrypt = ShaHash.GetShaHash(request.password);
            string concatPasswordKeys = String.Concat(CONST.KEY_CRYPTAGE, mdpCrypt, CONST.KEY_CRYPTAGE);
            string stringPasswordCrypt = ShaHash.GetShaHash(concatPasswordKeys);

            string emailCrypt = ShaHash.GetShaHash(request.email);
            string concatEmailKeys = String.Concat(CONST.KEY_CRYPTAGE, emailCrypt, CONST.KEY_CRYPTAGE);
            string stringEmailCrypt = ShaHash.GetShaHash(concatEmailKeys);

            request.password = stringPasswordCrypt;
            request.email = stringEmailCrypt;
            #endregion

            try
            {
                request.nickname = request.nickname[0].ToString().ToUpper() + request.nickname.Substring(1).ToLower();
                bool estInscrit = bdd.Registration(request);
                responseFront.response = MSG.INSCRIPTION_OK;
                return responseFront;
            }catch(Exception e)
            {
                log.Warn("Erreur durant l'inscription : " + e.Message);
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_FAILED;
                return responseFront;
            }
        }

    }
}
