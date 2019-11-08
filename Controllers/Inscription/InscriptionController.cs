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

            //Test de la taille de MDP
            if (request.registerAccount.registerPassword.Length < CONST.MINIMUM_PASSWORD_LENGTH)
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.passwordNoMinLength;
                return responseFront;
            }

            //Test de la taille du PSEUDO
            if (request.registerUser.registerNickname.Length > CONST.MAXIMUM_NICKNAME_LENGTH)
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.nicknameMaxLength;
                return responseFront;
            }

            //Test du contenu du PSEUDO (mots interdis)
            //string[] listeMotsInterdis = CONST.MOTS_INTERDIS.Split('|');
            bool test = false;
            List<String> listeMotsInterdits = bdd.RetrieveListOfForbiddenWord();

            for (int i = 0; i < listeMotsInterdits.Count(); i++)
            {
                string motInterditNormaliser = listeMotsInterdits[i].ToString().ToLower();
                string pseudoNormaliser = request.registerUser.registerNickname.ToLower();
                if (pseudoNormaliser.Contains(motInterditNormaliser))
                {
                    test = true;
                }
            }

            if (test)
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.nicknameWordForbidden;
                return responseFront;
            }

            //Test de la validation d'une adresse Email
            if (!request.registerAccount.registerEmail.Contains("@"))
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.mailInvalid;
                return responseFront;
            }

            //Test si le PSEUDO contient plus de 2 espaces
            if (request.registerUser.registerNickname.Contains("  "))
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.nicknameTwoSpaces;
                return responseFront;
            }

            //Test si le PSEUDO contient plus de 2 -
            if (request.registerUser.registerNickname.Contains("--"))
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.nicknameTwoDashes;
                return responseFront;
            }
            #endregion

            #region Cryptage du MDP et de l'Email
            string mdpCrypt = ShaHash.GetShaHash(request.registerAccount.registerPassword);
            string concatPasswordKeys = String.Concat(CONST.KEY_CRYPTAGE, mdpCrypt, CONST.KEY_CRYPTAGE);
            string stringPasswordCrypt = ShaHash.GetShaHash(concatPasswordKeys);

            string emailCrypt = ShaHash.GetShaHash(request.registerAccount.registerEmail);
            string concatEmailKeys = String.Concat(CONST.KEY_CRYPTAGE, emailCrypt, CONST.KEY_CRYPTAGE);
            string stringEmailCrypt = ShaHash.GetShaHash(concatEmailKeys);

            request.registerAccount.registerPassword = stringPasswordCrypt;
            request.registerAccount.registerEmail = stringEmailCrypt;
            #endregion

            //Test si l'adresse Email existe ou non
            if (bdd.TestIfEmailExist(request.registerAccount.registerEmail))
            {
                responseFront.hasError = false;
                responseFront.response = RegisterEnum.mailAlreadyExist;
                return responseFront;
            }

            try
            {
                request.registerUser.registerNickname = request.registerUser.registerNickname[0].ToString().ToUpper() + request.registerUser.registerNickname.Substring(1).ToLower();
                bool estInscrit = bdd.Registration(request);
                responseFront.response = RegisterEnum.ok;
                return responseFront;
            }catch(Exception e)
            {
                log.Warn("Erreur durant l'inscription : " + e.Message);
                responseFront.hasError = true;
                responseFront.error = RegisterEnum.nok;
                return responseFront;
            }
        }

    }
}
