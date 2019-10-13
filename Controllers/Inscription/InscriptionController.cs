using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;
using LauncherBack.Helpers;
using LauncherBack.Controllers.Connexion;

namespace LauncherBack.Controllers.Inscription
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InscriptionController : Controller
    {
        Bdd bdd = new Bdd();
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("Inscription")]
        public ResponseFront Inscription([FromBody] RequestFrontInscription request)
        {
            #region Critères d'acceptances
            //On test les critères d'acceptances de l'Email / Password / Pseudo

            //Test si l'adresse Email existe ou non
            if (bdd.EmailExist(request.email))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_EMAIL_EXIST;
                return responseFront;
            }

            //Test de la taille de MDP
            if (request.password.Length < CONST.LONGUEUR_PASSWORD_MIN)
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PASSWORD_COURT;
                return responseFront;
            }

            //Test de la taille du PSEUDO
            if (request.pseudo.Length > CONST.LONGUEUR_PSEUDO_MAX)
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_LONG;
                return responseFront;
            }

            //Test du contenu du PSEUDO (mots interdis)
            //string[] listeMotsInterdis = CONST.MOTS_INTERDIS.Split('|');
            bool test = false;
            List<String> listeMotsInterdits = bdd.RecupListeMotsInterdits();

            for (int i = 0; i < listeMotsInterdits.Count(); i++)
            {
                string motInterditNormaliser = listeMotsInterdits[i].ToString().ToLower();
                string pseudoNormaliser = request.pseudo.ToLower();
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
            if (request.pseudo.Contains("  "))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_2_ESPACES;
                return responseFront;
            }

            //Test si le PSEUDO contient plus de 2 -
            if (request.pseudo.Contains("--"))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_PSEUDO_2_TIRETS;
                return responseFront;
            }
            #endregion

            #region Cryptage du MDP
            string mdpCrypt = ShaHash.GetShaHash(request.password);
            string concatPasswordKeys = String.Concat(CONST.KEY_CRYPTAGE, mdpCrypt, CONST.KEY_CRYPTAGE);
            string stringCrypt = ShaHash.GetShaHash(concatPasswordKeys);

            request.password = stringCrypt;
            #endregion

            try
            {
                request.pseudo = request.pseudo[0].ToString().ToUpper() + request.pseudo.Substring(1).ToLower();
                bool estInscrit = bdd.Inscription(request);
                responseFront.response = MSG.INSCRIPTION_OK;
                return responseFront;
            }catch(Exception e)
            {
                Console.WriteLine("Erreur durant l'inscription : " + e.Message);
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_FAILED;
                return responseFront;
            }
        }

    }
}
