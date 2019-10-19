namespace LauncherBack.Helpers
{
    public class Constantes
    {
        public const int LONGUEUR_TOKEN = 254;
        public const string KEY_CRYPTAGE = "a2-6b25c2+4D23E%22f21g20$H19I18-j*17k16l15m14N,13O.12p1:;1q10r9S8T7U6v5w4x3_Y2z1";
        public const int LONGUEUR_PASSWORD_MIN = 8;
        public const int LONGUEUR_PSEUDO_MAX = 16;

        public static int envTravail;

        public const string FORMAT_DATE = "yyyy-MM-dd HH:mm:ss";

        //Liste des status possible pour un joueur
        public const string EN_LIGNE = "En ligne";
        public const string HORS_LIGNE = "Hors ligne";
        public const string OCCUPE = "Occupée";
        public const string INVISIBLE = "Invisible";
        public const string ABSENT = "Absent";

        public const string URL_SERVER_PROD = "http://92.222.80.11:5000";
        public const string URL_SERVER_DEV = "https://localhost:5001";
    }
}
