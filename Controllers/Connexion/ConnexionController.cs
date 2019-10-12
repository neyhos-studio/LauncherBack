
using LauncherBack.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using CONST = LauncherBack.Helpers.Constantes;

namespace LauncherBack.Controllers.Connexion
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnexionController : ControllerBase
    {
        Bdd bdd = new Bdd();

        [HttpPost]
        [ActionName("Inscription")]
        public ResponseFront Connexion([FromBody] RequestFront request)
        {
            bool exist = bdd.Connexion(request);
            ResponseFront responseFront = new ResponseFront();

            if (exist)
            {
                int idAccount = bdd.RecupIdAccount(request);
                string tokenClient = generationToken(CONST.longueurToken);
                string tokenServer = generationToken(CONST.longueurToken);

                bdd.InsertToken(idAccount, tokenServer, tokenClient);
                bdd.PassageEnLigne(idAccount);

                responseFront.response = tokenClient;
                return responseFront;
            }else
            {
                responseFront.hasError = true;
                responseFront.error = "Compte introuvable";
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
