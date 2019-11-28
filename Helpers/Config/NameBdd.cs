using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Inscription;
using LauncherBack.Controllers.Utilisateur;
using LauncherBack.Controllers.Social;
using MySql.Data.MySqlClient;
using CONST = LauncherBack.Helpers.Constantes;


namespace LauncherBack.Helpers.Config
{
    public class NameBdd : INameBdd
    {
        #region CONSTANTES
        
        private const string PREFIX = "NS";
        private const string PREFIX_NAME_FIELD = "_";
        //string.format
        #region NS_ACCOUNTS
            private static readonly string ENTITY_ACCOUNT = "ACCOUNT".ToUpper();
            private static readonly string NAME_TABLE_ACCOUNT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_ACCOUNT);
            private static readonly string NAME_FIELD_ACCOUNT_ID = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_ACCOUNT_EMAIL = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "EMAIL");
            private static readonly string NAME_FIELD_ACCOUNT_PASSWORD = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "PASSWORD");
        #endregion

        #region NS_UTILISATEURS
            private static readonly string ENTITY_UTILISATEUR = "UTILISATEUR".ToUpper();
            private static readonly string NAME_TABLE_UTILISATEUR = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_UTILISATEUR);
            private static readonly string NAME_FIELD_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_UTILISATEUR_ID_ACCOUNT = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ID_ACCOUNT");
            private static readonly string NAME_FIELD_UTILISATEUR_PSEUDO = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "PSEUDO");
            private static readonly string NAME_FIELD_UTILISATEUR_ETAT = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ETAT");
            private static readonly string NAME_FIELD_UTILISATEUR_AVATAR = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "AVATAR");
        #endregion

        #region NS_AVATAR
        private static readonly string ENTITY_AVATAR = "AVATAR".ToUpper();
        private static readonly string NAME_TABLE_AVATAR = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_AVATAR);
        private static readonly string NAME_FIELD_AVATAR_ID = String.Format("{0}{1}{2}", ENTITY_AVATAR, PREFIX_NAME_FIELD, "ID");
        private static readonly string NAME_FIELD_AVATAR_URL = String.Format("{0}{1}{2}", ENTITY_AVATAR, PREFIX_NAME_FIELD, "URL");
        #endregion

        #region NS_ETATS_UTILISATEUR
        private static readonly string ENTITY_ETAT_UTILISATEUR = "ETAT_UTILISATEUR".ToUpper();
            private static readonly string NAME_TABLE_ETAT_UTILISATEUR = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_ETAT_UTILISATEUR);
            private static readonly string NAME_FIELD_ETAT_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_ETAT_UTILISATEUR_LIBELLE = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "LIBELLE");
            private static readonly string NAME_FIELD_ETAT_UTILISATEUR_COLOR = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "COLOR");
        #endregion

        #region NS_TOKENS
            private static readonly string ENTITY_TOKEN = "TOKEN".ToUpper();
            private static readonly string NAME_TABLE_TOKEN = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_TOKEN);
            private static readonly string NAME_FIELD_TOKEN_ID = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_TOKEN_ACCOUNT_ID = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "ACCOUNT_ID");
            private static readonly string NAME_FIELD_TOKEN_TOKEN_SERVER = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "TOKEN_SERVER");
            private static readonly string NAME_FIELD_TOKEN_TOKEN_CLIENT = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "TOKEN_CLIENT");
            private static readonly string NAME_FIELD_TOKEN_DATE_CREATION = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "DATE_CREATION");
        #endregion

        #region NS_BANNISSEMENTS
            private static readonly string ENTITY_BANNISSEMENT = "BANNISSEMENT".ToUpper();
            private static readonly string NAME_TABLE_BANNISSEMENT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_BANNISSEMENT);
            private static readonly string NAME_FIELD_BANNISSEMENT_ID = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_BANNISSEMENT_ACCOUNT = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "ACCOUNT");
            private static readonly string NAME_FIELD_BANNISSEMENT_DATE_DEBUT = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DATE_DEBUT");
            private static readonly string NAME_FIELD_BANNISSEMENT_DUREE = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DUREE");
            private static readonly string NAME_FIELD_BANNISSEMENT_DATE_FIN = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DATE_FIN");
            private static readonly string NAME_FIELD_BANNISSEMENT_RAISON = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "RAISON");
        #endregion

        #region NS_JEUX
            private static readonly string ENTITY_JEU = "JEU".ToUpper();
            private static readonly string NAME_TABLE_JEU = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_JEU);
            private static readonly string NAME_FIELD_JEU_ID = String.Format("{0}{1}{2}", ENTITY_JEU, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_JEU_NAME = String.Format("{0}{1}{2}", ENTITY_JEU, PREFIX_NAME_FIELD, "NAME");
        #endregion

        #region NS_UTILISATEUR_JEU
            private static readonly string ENTITY_UTILISATEUR_JEU = "UTILISATEUR_JEU".ToUpper();
            private static readonly string NAME_TABLE_UTILISATEUR_JEU = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_UTILISATEUR_JEU);
            private static readonly string NAME_FIELD_UTILISATEUR_JEU_ID = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_UTILISATEUR_JEU_JEU = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "JEU");
            private static readonly string NAME_FIELD_UTILISATEUR_JEU_UTILISATEUR = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "UTILISATEUR");
        #endregion

        #region NS_SOCIAL
            private static readonly string ENTITY_SOCIAL = "SOCIAL".ToUpper();
            private static readonly string NAME_TABLE_SOCIAL = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_SOCIAL);
            private static readonly string NAME_FIELD_SOCIAL_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_SOCIAL_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "UTILISATEUR_ID");
            private static readonly string NAME_FIELD_SOCIAL_APOURAMI_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "APOURAMI_ID");
        #endregion

        #region NS_MOTS_iNTERDIS
            private static readonly string ENTITY_MOT_INTERDIT = "MOT_INTERDIT".ToUpper();
            private static readonly string NAME_TABLE_MOT_INTERDIT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_MOT_INTERDIT);
            private static readonly string NAME_FIELD_MOT_INTERDIT_ID = String.Format("{0}{1}{2}", ENTITY_MOT_INTERDIT, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_MOT_INTERDIT_LIBELLE = String.Format("{0}{1}{2}", ENTITY_MOT_INTERDIT, PREFIX_NAME_FIELD, "LIBELLE");
        #endregion

        #region NS_NEWS
            private static readonly string ENTITY_NEWS = "NEWS".ToUpper();
            private static readonly string NAME_TABLE_NEWS = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_NEWS);
            private static readonly string NAME_FIELD_NEWS_ID = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_NEWS_TITLE = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "TITLE");
            private static readonly string NAME_FIELD_NEWS_DATE = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "DATE");
            private static readonly string NAME_FIELD_NEWS_IMAGE = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "IMAGE");
            private static readonly string NAME_FIELD_NEWS_CONTENT = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "CONTENT");
            private static readonly string NAME_FIELD_NEWS_CATEG = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "CATEG");
            private static readonly string NAME_FIELD_NEWS_PUBLISH = String.Format("{0}{1}{2}", ENTITY_NEWS, PREFIX_NAME_FIELD, "PUBLISH");
        #endregion

        #region NS_NEWS_CATEG
        private static readonly string ENTITY_NEWS_CATEG = "NEWS_CATEG".ToUpper();
            private static readonly string NAME_TABLE_NEWS_CATEG = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_NEWS_CATEG);
            private static readonly string NAME_FIELD_NEWS_CATEG_ID = String.Format("{0}{1}{2}", ENTITY_NEWS_CATEG, PREFIX_NAME_FIELD, "ID");
            private static readonly string NAME_FIELD_NEWS_CATEG_LIBELLE = String.Format("{0}{1}{2}", ENTITY_NEWS_CATEG, PREFIX_NAME_FIELD, "LIBELLE");
        #endregion

        #region NS_TCHAT_MESSAGE
        private static readonly string ENTITY_NS_TCHAT_MESSAGE = "TCHAT_MESSAGE".ToUpper();
        private static readonly string NAME_TABLE_TCHAT_MESSAGE = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_NS_TCHAT_MESSAGE);
        private static readonly string NAME_FIELD_TCHAT_MESSAGE_ID = String.Format("{0}{1}{2}", ENTITY_NS_TCHAT_MESSAGE, PREFIX_NAME_FIELD, "ID");
        private static readonly string NAME_FIELD_TCHAT_MESSAGE_DATETIME = String.Format("{0}{1}{2}", ENTITY_NS_TCHAT_MESSAGE, PREFIX_NAME_FIELD, "DATETIME");
        private static readonly string NAME_FIELD_TCHAT_MESSAGE_MESSAGE = String.Format("{0}{1}{2}", ENTITY_NS_TCHAT_MESSAGE, PREFIX_NAME_FIELD, "MESSAGE");
        private static readonly string NAME_FIELD_TCHAT_MESSAGE_FROM = String.Format("{0}{1}{2}", ENTITY_NS_TCHAT_MESSAGE, PREFIX_NAME_FIELD, "FROM");
        private static readonly string NAME_FIELD_TCHAT_MESSAGE_TO = String.Format("{0}{1}{2}", ENTITY_NS_TCHAT_MESSAGE, PREFIX_NAME_FIELD, "TO");
        #endregion

        #endregion

        #region METHODES
        public string retrieveUserID(RequestFrontUtilisateur token)
        {
            return String.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                NAME_TABLE_TOKEN,
                NAME_FIELD_TOKEN_TOKEN_CLIENT,
                token.token);
        }

        public int retrieveUserIdInt(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_TOKEN_ACCOUNT_ID].ToString());
        }
        public string connection(RequestFrontConnexion request)
        {
            return String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}' AND {3} = '{4}'",
                NAME_TABLE_ACCOUNT,
                NAME_FIELD_ACCOUNT_EMAIL,
                request.email,
                NAME_FIELD_ACCOUNT_PASSWORD,
                request.password);
        }
        public string retrieveUserIdConnection(RequestFrontConnexion request)
        {
            return String.Format("SELECT {0} FROM {1} WHERE {2} = '{3}' AND {4} = '{5}'",
                NAME_FIELD_ACCOUNT_ID,
                NAME_TABLE_ACCOUNT,
                NAME_FIELD_ACCOUNT_EMAIL,
                request.email,
                NAME_FIELD_ACCOUNT_PASSWORD,
                request.password);
        }
        public int retrieveFieldAccountIdConnection(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_ACCOUNT_ID].ToString());
        }
        public string addToken(int idAccount, string tokenServer, string tokenClient)
        {
            return String.Format("INSERT INTO {0} ({1},{2},{3},{4}) VALUES ({5}, '{6}', '{7}', '{8}')",
                NAME_TABLE_TOKEN,
                NAME_FIELD_TOKEN_ACCOUNT_ID,
                NAME_FIELD_TOKEN_TOKEN_SERVER,
                NAME_FIELD_TOKEN_TOKEN_CLIENT,
                NAME_FIELD_TOKEN_DATE_CREATION,
                idAccount,
                tokenServer,
                tokenClient,
                DateTime.Now.ToString(CONST.DATE_TIME_FORMAT));
        }
        public string nowOnline(int idAccount)
        {
            return String.Format("UPDATE {0} SET {1} = 2 WHERE {2} = {3}",
                NAME_TABLE_UTILISATEUR,
                NAME_FIELD_UTILISATEUR_ETAT,
                NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                idAccount);
        }
        public string testIfUserBan(int idAccount)
        {
            return String.Format("SELECT * FROM {0} WHERE {1} = {2} AND {3} > '{4}'",
                NAME_TABLE_BANNISSEMENT,
                NAME_FIELD_BANNISSEMENT_ACCOUNT,
                idAccount,
                NAME_FIELD_BANNISSEMENT_DATE_FIN,
                DateTime.Now.ToString(CONST.DATE_TIME_FORMAT));
        }
        public DateTime banishmentStart(MySqlDataReader dataReader)
        {
            return DateTime.Parse(dataReader[NAME_FIELD_BANNISSEMENT_DATE_DEBUT].ToString());
        }
        public int banishmentDuring(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_BANNISSEMENT_DUREE].ToString());
        }
        public DateTime banishmentEnd(MySqlDataReader dataReader)
        {
            return DateTime.Parse(dataReader[NAME_FIELD_BANNISSEMENT_DATE_FIN].ToString());
        }
        public string banishmentReason(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_BANNISSEMENT_RAISON].ToString();
        }
        public string registrationAddAccount(RequestFrontInscription request)
        {
            return String.Format("INSERT INTO {0} ({1}, {2}) VALUES ('{3}', '{4}')",
                NAME_TABLE_ACCOUNT,
                NAME_FIELD_ACCOUNT_EMAIL,
                NAME_FIELD_ACCOUNT_PASSWORD,
                request.registerAccount.registerEmail,
                request.registerAccount.registerPassword);
        }
        public string registrationRecupIdAccount(RequestFrontInscription request)
        {
            return String.Format("SELECT MAX({0}) as MAX_ID FROM {1} WHERE {2} = '{3}'",
                NAME_FIELD_ACCOUNT_ID,
                NAME_TABLE_ACCOUNT,
                NAME_FIELD_ACCOUNT_EMAIL,
                request.registerAccount.registerEmail);
        }
        public string registrationAddUser(int idAccount, RequestFrontInscription request)
        {
            return String.Format("INSERT INTO {0} ({1}, {2}, {3}) VALUES ({4}, '{5}', {6})",
                    NAME_TABLE_UTILISATEUR,
                    NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                    NAME_FIELD_UTILISATEUR_PSEUDO,
                    NAME_FIELD_UTILISATEUR_AVATAR,
                    idAccount,
                    request.registerUser.registerNickname,
                    0);
        }
        public string testIfEmailExist(string email)
        {
            return String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'",
                NAME_TABLE_ACCOUNT,
                NAME_FIELD_ACCOUNT_EMAIL,
                email);
        }
        public string retrieveForbiddenWordList()
        {
            return String.Format("SELECT * FROM {0}",
                NAME_TABLE_MOT_INTERDIT);
        }
        public string fieldForbiddenWord(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_MOT_INTERDIT_LIBELLE].ToString();
        }
        public string insertForbiddenWord(string motInterdit)
        {
            return String.Format("INSERT INTO {0} ({1}) VALUES ('{2}')",
                NAME_TABLE_MOT_INTERDIT,
                NAME_FIELD_MOT_INTERDIT_LIBELLE,
                motInterdit);
        }
        public string retrieveUser(int idAccount)
        {
            return String.Format("SELECT * FROM {0}, {1} WHERE {2}.{3} = {4}.{5} AND {6} = '{7}'",
               NAME_TABLE_UTILISATEUR,
               NAME_TABLE_AVATAR,
               NAME_TABLE_UTILISATEUR,
               NAME_FIELD_UTILISATEUR_AVATAR,
               NAME_TABLE_AVATAR,
               NAME_FIELD_AVATAR_ID,
               NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
               idAccount);
        }
        public string retrieveNicknameUser(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_UTILISATEUR_PSEUDO].ToString();
        }
        public string retrieveAvatarUser(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_AVATAR_URL].ToString();
        }
        public int retrieveStatusUser(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_UTILISATEUR_ETAT].ToString());
        }
        public string banUser(int idAccount, DateTime startDate, int during, DateTime dateFin, string reason)
        {
            return String.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}) VALUES ({6}, '{7}', {8}, '{9}', '{10}')",
                NAME_TABLE_BANNISSEMENT,
                NAME_FIELD_BANNISSEMENT_ACCOUNT,
                NAME_FIELD_BANNISSEMENT_DATE_DEBUT,
                NAME_FIELD_BANNISSEMENT_DUREE,
                NAME_FIELD_BANNISSEMENT_DATE_FIN,
                NAME_FIELD_BANNISSEMENT_RAISON,
                idAccount,
                startDate.ToString(CONST.DATE_TIME_FORMAT),
                during,
                dateFin.ToString(CONST.DATE_TIME_FORMAT),
                reason.Replace("'", "''"));
        }
        public string retrieveIdAccountFriend(int idAccount)
        {
            return String.Format("SELECT {0} FROM {1} WHERE {2} = {3}",
                NAME_FIELD_SOCIAL_APOURAMI_ID,
                NAME_TABLE_SOCIAL,
                NAME_FIELD_SOCIAL_UTILISATEUR_ID,
                idAccount);
        }
        public int retrieveIdFriend(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_SOCIAL_APOURAMI_ID].ToString());
        }
        public string retrieveInfoFriends(List<int> friendListId, int i)
        {
            return String.Format("SELECT * FROM {0}, {1} WHERE {2}.{3} = {4}.{5} AND {6} = {7}",
                    NAME_TABLE_UTILISATEUR,
                    NAME_TABLE_AVATAR,
                    NAME_TABLE_UTILISATEUR,
                    NAME_FIELD_UTILISATEUR_AVATAR,
                    NAME_TABLE_AVATAR,
                    NAME_FIELD_AVATAR_ID,
                    NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                    friendListId[i]);
        }
        public string retrieveNicknameFriend(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_UTILISATEUR_PSEUDO].ToString();
        }
        public string retrieveStatusFriend(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_UTILISATEUR_ETAT].ToString();
        }
        public string retrieveAvatarFriend(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_AVATAR_URL].ToString();
        }
        
        public string retrieveGameListUser(int idAccount)
        {
            return String.Format("SELECT {0} FROM {1} WHERE {2} = {3}",
                NAME_FIELD_UTILISATEUR_JEU_JEU,
                NAME_TABLE_UTILISATEUR_JEU,
                NAME_FIELD_UTILISATEUR_JEU_UTILISATEUR,
                idAccount);
        }
        public int retrieveFieldIdGame(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_UTILISATEUR_JEU_JEU].ToString());
        }
        public string retrieveGameInfos(List<int> userGameList, int i)
        {
            return String.Format("SELECT * FROM {0} WHERE {1} = {2}",
                    NAME_TABLE_JEU,
                    NAME_FIELD_JEU_ID,
                    userGameList[i]);
        }
        public string retrieveNameGame(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_JEU_NAME].ToString();
        }
        public string retrieveGameList()
        {
            return String.Format("SELECT * FROM {0}",
                NAME_TABLE_JEU);
        }
        public int retrieveIdGame(MySqlDataReader dataReader)
        {
            return int.Parse(dataReader[NAME_FIELD_JEU_ID].ToString());
        }
        public string retrieveTitleGame(MySqlDataReader dataReader)
        {
            return dataReader[NAME_FIELD_JEU_NAME].ToString();
        }
        public string retrieveNewsList()
        {
            return String.Format("SELECT * FROM {0}, {1} WHERE {2}.{3} = {4}.{5} AND {6} = 1",
                NAME_TABLE_NEWS,
                NAME_TABLE_NEWS_CATEG,
                NAME_TABLE_NEWS,
                NAME_FIELD_NEWS_CATEG,
                NAME_TABLE_NEWS_CATEG,
                NAME_FIELD_NEWS_CATEG_ID,
                NAME_FIELD_NEWS_PUBLISH);
        }
        public Collection<Object> retrieveFieldsNews(MySqlDataReader dataReader)
        {
            Collection<Object> fieldsListe = new Collection<object>();

            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_ID]);
            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_TITLE]);
            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_DATE]);
            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_IMAGE]);
            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_CONTENT]);
            fieldsListe.Add(dataReader[NAME_FIELD_NEWS_CATEG_LIBELLE]);

            return fieldsListe;
        }

        public string disconectionUser(int idAccount)
        {
            return String.Format("UPDATE {0} SET {1} = 1 WHERE {2} = {3}",
                NAME_TABLE_UTILISATEUR,
                NAME_FIELD_UTILISATEUR_ETAT,
                NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                idAccount);
        }

        public string nowBannedUser(int idAccount)
        {
            return String.Format("UPDATE {0} SET {1} = 6 WHERE {2} = {3}",
                NAME_TABLE_UTILISATEUR,
                NAME_FIELD_UTILISATEUR_ETAT,
                NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                idAccount);
        }

        public string deleteTokenUser(string token)
        {
            return String.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                NAME_TABLE_TOKEN,
                NAME_FIELD_TOKEN_TOKEN_CLIENT,
                token);
        }

        public string sendMessage(Message requestFrontMessage)
        {
            return String.Format("INSERT INTO {0} ({1},{2},{3},{4}) VALUES ('{5}', '{6}', {7}, {8})",
                NAME_TABLE_TCHAT_MESSAGE,
                NAME_FIELD_TCHAT_MESSAGE_DATETIME,
                NAME_FIELD_TCHAT_MESSAGE_MESSAGE,
                NAME_FIELD_TCHAT_MESSAGE_FROM,
                NAME_FIELD_TCHAT_MESSAGE_TO,
                requestFrontMessage.messageDateTime.ToString(CONST.DATE_TIME_FORMAT),
                requestFrontMessage.message,
                requestFrontMessage.messageFrom,
                requestFrontMessage.messageTo);
        }

        #region CLEAN DATABASE
        public List<string> cleanDatabase()
        {
            List<string> listRequest = new List<string>();

            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_UTILISATEUR_JEU));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_UTILISATEUR_JEU));
            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_SOCIAL));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_SOCIAL));
            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_BANNISSEMENT));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_BANNISSEMENT));
            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_TOKEN));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_TOKEN));
            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_UTILISATEUR));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_UTILISATEUR));
            listRequest.Add(String.Format("DELETE FROM {0}", NAME_TABLE_ACCOUNT));
            listRequest.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_ACCOUNT));

            return listRequest;
        }
        public List<string> cleanTokens()
        {
            List<string> listeRequestCleanTokens = new List<string>();
            listeRequestCleanTokens.Add(String.Format("DELETE FROM {0}", NAME_TABLE_TOKEN));
            listeRequestCleanTokens.Add(String.Format("ALTER TABLE {0} AUTO_INCREMENT = 1", NAME_TABLE_TOKEN));

            return listeRequestCleanTokens;

        }
        public string disconnectAllUsers()
        {
            return String.Format("UPDATE {0} SET {1} = {2}", NAME_TABLE_UTILISATEUR, NAME_FIELD_UTILISATEUR_ETAT, 1);
        }
        
        #endregion

        #endregion
    }
}
