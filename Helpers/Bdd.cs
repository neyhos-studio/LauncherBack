using System;
using System.Collections.Generic;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Inscription;
using LauncherBack.Controllers.Utilisateur;
using MySql.Data.MySqlClient;
using MSG = LauncherBack.Helpers.Messages;
using CONST = LauncherBack.Helpers.Constantes;
using CONST_BDD = LauncherBack.Helpers.Config.NameBdd;
using LauncherBack.Controllers.Social;
using LauncherBack.Controllers.Games;

namespace LauncherBack.Helpers
{
    public class Bdd
    {
        private MySqlConnection connection;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor
        public Bdd(string server, string database, string login, string password)
        {
            Initialize(server, database, login, password);
        }

        //Initialize values
        private void Initialize(string server, string database, string login, string password)
        {
            string connectionString;
            connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", server, database, login, password);

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {

            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Error("Connexion à la BDD impossible !");
                switch (ex.Number)
                {
                    case 0:
                        log.Warn(MSG.BDD_CANNOT_CONNECT_SERVER);
                        break;

                    case 1045:
                        log.Warn(MSG.BDD_INVALID_USERNAME_OR_PASSWORD);
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #region CONNEXION

        //ACCOUNT EXIST
        public int Connection(RequestFrontConnexion request)
        {
            string queryConnexion = String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}' AND {3} = '{4}'", 
                CONST_BDD.NAME_TABLE_ACCOUNT, 
                CONST_BDD.NAME_FIELD_ACCOUNT_EMAIL, 
                request.email, 
                CONST_BDD.NAME_FIELD_ACCOUNT_PASSWORD, 
                request.password);
            int Count = 0;

            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(queryConnexion, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //RECUP ID ACCOUNT
        public int RetrieveIdAccount(RequestFrontConnexion request)
        {
            string queryRecupID = String.Format("SELECT {0} FROM {1} WHERE {2} = '{3}' AND {4} = '{5}'", 
                CONST_BDD.NAME_FIELD_ACCOUNT_ID, 
                CONST_BDD.NAME_TABLE_ACCOUNT,
                CONST_BDD.NAME_FIELD_ACCOUNT_EMAIL,
                request.email,
                CONST_BDD.NAME_FIELD_ACCOUNT_PASSWORD,
                request.password);
            int id = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryRecupID, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                while (dataReader.Read())
                {
                    id = int.Parse(dataReader[CONST_BDD.NAME_FIELD_ACCOUNT_ID] + "");
                }

                dataReader.Close();

                this.CloseConnection();

                return id;
            } else
            {
                return 0;
            }

            
        }

        //INSERT TOKEN
        public void InsertToken(int idAccount, string tokenServer, string tokenClient)
        {
            string queryInsertToken = String.Format("INSERT INTO {0} ({1},{2},{3},{4}) VALUES ({5}, '{6}', '{7}', '{8}')",
                CONST_BDD.NAME_TABLE_TOKEN,
                CONST_BDD.NAME_FIELD_TOKEN_ACCOUNT_ID,
                CONST_BDD.NAME_FIELD_TOKEN_TOKEN_SERVER,
                CONST_BDD.NAME_FIELD_TOKEN_TOKEN_CLIENT,
                CONST_BDD.NAME_FIELD_TOKEN_DATE_CREATION,
                idAccount,
                tokenServer, 
                tokenClient,
                DateTime.Now.ToString(CONST.DATE_FORMAT));

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryInsertToken, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            else
            {
            }
        }

        //PASSAGE EN LIGNE
        public void NowOnline(int idAccount)
        {
            string queryPassageEnLigne = String.Format("UPDATE {0} SET {1} = 2 WHERE {2} = {3}",
                CONST_BDD.NAME_TABLE_UTILISATEUR,
                CONST_BDD.NAME_FIELD_UTILISATEUR_ETAT,
                CONST_BDD.NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                idAccount);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryPassageEnLigne, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            else
            {
            }
        }

        public Banishment TestIfUserBan(int idAccount)
        {
            Banishment banishment = new Banishment();

            string queryIfBanned = String.Format("SELECT * FROM {0} WHERE {1} = {2} AND {3} > '{4}'",
                CONST_BDD.NAME_TABLE_BANNISSEMENT,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_ACCOUNT,
                idAccount,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_DATE_FIN,
                DateTime.Now.ToString(CONST.DATE_FORMAT));

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryIfBanned, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    banishment.hasBanned = true;
                    banishment.start = DateTime.Parse(dataReader[CONST_BDD.NAME_FIELD_BANNISSEMENT_DATE_DEBUT] + "");
                    banishment.during = int.Parse(dataReader[CONST_BDD.NAME_FIELD_BANNISSEMENT_DUREE] + "");
                    banishment.end = DateTime.Parse(dataReader[CONST_BDD.NAME_FIELD_BANNISSEMENT_DATE_FIN] + "");
                    banishment.reason = dataReader[CONST_BDD.NAME_FIELD_BANNISSEMENT_RAISON] + "";
                }

                dataReader.Close();

                this.CloseConnection();
                return banishment;
            }
            else
            {
                return banishment;
            }
        }

        #endregion

        #region INSCRIPTION 

        public bool Registration(RequestFrontInscription request)
        {
            string queryAddAccount = String.Format("INSERT INTO {0} ({1}, {2}) VALUES ('{3}', '{4}')",
                CONST_BDD.NAME_TABLE_ACCOUNT,
                CONST_BDD.NAME_FIELD_ACCOUNT_EMAIL,
                CONST_BDD.NAME_FIELD_ACCOUNT_PASSWORD,
                request.email,
                request.password);

            string quertRecupIdAccount = String.Format("SELECT MAX({0}) as MAX_ID FROM {1} WHERE {2} = '{3}'",
                CONST_BDD.NAME_FIELD_ACCOUNT_ID,
                CONST_BDD.NAME_TABLE_ACCOUNT,
                CONST_BDD.NAME_FIELD_ACCOUNT_EMAIL,
                request.email);

            int id = 0;
            bool isRegistered = false;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryAddAccount, connection);
                cmd.ExecuteNonQuery();

                MySqlCommand recupId = new MySqlCommand(quertRecupIdAccount, connection);
                recupId.ExecuteNonQuery();
                MySqlDataReader dataReader = recupId.ExecuteReader();

                while (dataReader.Read())
                {
                    id = int.Parse(dataReader["MAX_ID"] + "");
                }

                dataReader.Close();

                string queryAddUtilisateur = String.Format("INSERT INTO {0} ({1}, {2}) VALUES ({3}, '{4}')",
                    CONST_BDD.NAME_TABLE_UTILISATEUR,
                    CONST_BDD.NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                    CONST_BDD.NAME_FIELD_UTILISATEUR_PSEUDO,
                    id,
                    request.nickname);

                MySqlCommand cmd2 = new MySqlCommand(queryAddUtilisateur, connection);
                cmd2.ExecuteNonQuery();

                this.CloseConnection();

                return isRegistered = true;

            }
            else
            {
                return isRegistered;
            }
        }

        public bool TestIfEmailExist(string email)
        {
            string queryEmailExist = String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'", 
                CONST_BDD.NAME_TABLE_ACCOUNT, 
                CONST_BDD.NAME_FIELD_ACCOUNT_EMAIL, 
                email);

            bool exist = false;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryEmailExist, connection);


                if (int.Parse(cmd.ExecuteScalar() + "") == 1)
                {
                    exist = true;
                }

                this.CloseConnection();
            }
            else
            {
                Console.WriteLine(MSG.BDD_ERREUR_CONNEXION_BDD);
            }

            return exist;
        }

        #endregion

        #region CONFIGURATION
        public List<String> RetrieveListOfForbiddenWord()
        {
            string queryRecupListeMotsInteerdits = String.Format("SELECT * FROM {0}",
                CONST_BDD.NAME_TABLE_MOT_INTERDIT);

            List<String> forbiddenWordList = new List<string>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryRecupListeMotsInteerdits, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                while (dataReader.Read())
                {
                    forbiddenWordList.Add(dataReader[CONST_BDD.NAME_FIELD_MOT_INTERDIT_LIBELLE].ToString());
                }

                dataReader.Close();

                this.CloseConnection();

                return forbiddenWordList;
            }
            else
            {
                return null;
            }


        }
        public bool InsertForbiddenWord(string motInterdit)
        {
            string queryAddMotInterdit = String.Format("INSERT INTO {0} ({1}) VALUES ('{2}')",
                CONST_BDD.NAME_TABLE_MOT_INTERDIT,
                CONST_BDD.NAME_FIELD_MOT_INTERDIT_LIBELLE,
                motInterdit);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryAddMotInterdit, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();

                return  true;
            }
            else
            {
                return  false;
            }
        }
        #endregion

        #region Utilisateur
        public int RetrieveUserId(RequestFrontUtilisateur request)
        {
            string queryRecupIdUtilisateur = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                CONST_BDD.NAME_TABLE_TOKEN,
                CONST_BDD.NAME_FIELD_TOKEN_TOKEN_CLIENT,
                request.token);

            int idAccount = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryRecupIdUtilisateur, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    idAccount = int.Parse(dataReader[CONST_BDD.NAME_FIELD_TOKEN_ACCOUNT_ID] + "");
                }

                dataReader.Close();

                this.CloseConnection();
                return idAccount;
            }
            else
            {
                return idAccount;
            }
        }

        public User RetrieveUser(int idAccount)
        {
            string queryRecuperationUtilisateur = String.Format("SELECT * FROM {0} WHERE {1} = {2}",
                CONST_BDD.NAME_TABLE_UTILISATEUR,
                CONST_BDD.NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                idAccount);

            User user = new User();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryRecuperationUtilisateur, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                while (dataReader2.Read())
                {
                    user.nickname = dataReader2[CONST_BDD.NAME_FIELD_UTILISATEUR_PSEUDO] + "";
                    switch (dataReader2[CONST_BDD.NAME_FIELD_UTILISATEUR_ETAT])
                    {
                        case 1:
                            user.status = CONST.OFFLINE;
                            break;
                        case 2:
                            user.status = CONST.ONLINE;
                            break;
                        case 3:
                            user.status = CONST.ABSENT;
                            break;
                        case 4:
                            user.status = CONST.BUSY;
                            break;
                        case 5:
                            user.status = CONST.INVISIBLE;
                            break;
                        default:
                            break;
                    }
                }

                dataReader2.Close();
                this.CloseConnection();

                return user;
            } else
            {
                return null;
            }
            }

        
        public void BanningUser(DateTime startDate, int during, string reason, int idAccount)
        {
            DateTime dateFin = startDate.AddDays(during);
            string queryBannirUtilisateur = String.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}) VALUES ({6}, '{7}', {8}, '{9}', '{10}')",
                CONST_BDD.NAME_TABLE_BANNISSEMENT,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_ACCOUNT,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_DATE_DEBUT,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_DUREE,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_DATE_FIN,
                CONST_BDD.NAME_FIELD_BANNISSEMENT_RAISON,
                idAccount,
                startDate.ToString(CONST.DATE_FORMAT),
                during,
                dateFin.ToString(CONST.DATE_FORMAT),
                reason.Replace("'", "''")); //TODO : remplacer le système d'échappement caractères

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryBannirUtilisateur, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            else
            {
            }
        }
        #endregion

        #region Social
        public List<Friend> RetrieveFriendListDatabase(int idAccount)
        {
            string queryRecupIdFriend = String.Format("SELECT {0} FROM {1} WHERE {2} = {3}",
                CONST_BDD.NAME_FIELD_SOCIAL_APOURAMI_ID,
                CONST_BDD.NAME_TABLE_SOCIAL,
                CONST_BDD.NAME_FIELD_SOCIAL_UTILISATEUR_ID,
                idAccount);

            
            List<Friend> friendList = new List<Friend>();
            List<int> friendsIdList = new List<int>();

            if (this.OpenConnection() == true)
            {
                //On récupère les ID des amis
                MySqlCommand cmd2 = new MySqlCommand(queryRecupIdFriend, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                while (dataReader.Read())
                {
                    friendsIdList.Add(int.Parse(dataReader[CONST_BDD.NAME_FIELD_SOCIAL_APOURAMI_ID].ToString()));
                }

                dataReader.Close();

                //On récupère les infos des amis

                for (int i = 0; i < friendsIdList.Count; i++)
                {
                    string queryRecupInfosFriends = String.Format("SELECT * FROM {0} WHERE {1} = {2}",
                    CONST_BDD.NAME_TABLE_UTILISATEUR,
                    CONST_BDD.NAME_FIELD_UTILISATEUR_ID_ACCOUNT,
                    friendsIdList[i]);

                    log.Debug(queryRecupInfosFriends);

                        MySqlCommand cmdQueryUser = new MySqlCommand(queryRecupInfosFriends, connection);
                        MySqlDataReader dataReaderUser = cmdQueryUser.ExecuteReader();

                        while (dataReaderUser.Read())
                        {
                            Friend friend = new Friend();

                            friend.nickname = dataReaderUser[CONST_BDD.NAME_FIELD_UTILISATEUR_PSEUDO].ToString();
                            friend.status = dataReaderUser[CONST_BDD.NAME_FIELD_UTILISATEUR_ETAT].ToString();

                            friendList.Add(friend);
                        }

                        dataReaderUser.Close();
                }

                this.CloseConnection();

                return friendList;
            }
            else
            {
                return friendList;
            }


        }

        #endregion

        #region GAME LIST
        public List<Game> RetrieveGameList()
        {
            List<Game> gameList = new List<Game>();
            

            string queryRetrieveGameList = String.Format("SELECT * FROM {0}",
                CONST_BDD.NAME_TABLE_JEU);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryRetrieveGameList, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Game game = new Game();

                    game.id = int.Parse(dataReader[CONST_BDD.NAME_FIELD_JEU_ID] + "");
                    game.title = dataReader[CONST_BDD.NAME_FIELD_JEU_NAME] + "";
                    game.price = 59.90;

                    gameList.Add(game);
                }

                dataReader.Close();

                this.CloseConnection();
                return gameList;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
