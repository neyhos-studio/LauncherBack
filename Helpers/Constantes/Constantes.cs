namespace LauncherBack.Helpers
{
    public class Constantes
    {
        public const int TOKEN_SIZE = 254;
        public const string KEY_CRYPTAGE = "a2-6b25c2+4D23E%22f21g20$H19I18-j*17k16l15m14N,13O.12p1:;1q10r9S8T7U6v5w4x3_Y2z1";
        public const int MINIMUM_PASSWORD_LENGTH = 8;
        public const int MAXIMUM_NICKNAME_LENGTH = 16;

        public static int WORK_ENV;

        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_FORMAT = "dd.MM.yyyy-HH:mm";

        //Liste des status possible pour un joueur
        public const string ONLINE = "En ligne";
        public const string OFFLINE = "Hors ligne";
        public const string BUSY = "Occupée";
        public const string INVISIBLE = "Invisible";
        public const string ABSENT = "Absent";

        public const string URL_SERVER_PROD = "http://92.222.80.11:5000";
        public const string URL_SERVER_DEV = "https://localhost:5001";

        public const int nbrSecret = 7;
    }
}
