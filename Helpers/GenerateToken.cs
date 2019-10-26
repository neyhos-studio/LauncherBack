using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Helpers
{
    public class GenerateToken
    {
        public static string generationToken(int longueurToken)
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
