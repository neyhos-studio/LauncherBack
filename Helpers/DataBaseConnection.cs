using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CONST = LauncherBack.Helpers.Constantes;
using CRED = LauncherBack.Helpers.Config.Credentials;

namespace LauncherBack.Helpers
{
    public class DataBaseConnection
    {
        public static Bdd databaseConnection()
        {
            Bdd bdd;

            //Configuration environnement de travail
            if (CONST.WORK_ENV == 0)
            {
                return bdd = new Bdd(CRED.SERVER_PROD, CRED.DATABASE_PROD, CRED.LOGIN_PROD, CRED.PASSWORD_PROD);
            }
            else
            {
                return bdd = new Bdd(CRED.SERVER_DEV, CRED.DATABASE_DEV, CRED.LOGIN_DEV, CRED.PASSWORD_DEV);
            }
        }
    }
}
