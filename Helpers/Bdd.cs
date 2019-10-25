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
using LauncherBack.Helpers.Config;
using LauncherBack.Controllers.News;
using System.Collections.ObjectModel;
using LauncherBack.Controllers.Configuration;

namespace LauncherBack.Helpers
{
    public class Bdd
    {
        private MySqlConnection connection;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private INameBdd nameBdd = new NameBdd();

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
                log.Error(ex);
                return false;
            }
        }

        #region CONNEXION

        //ACCOUNT EXIST
        public int Connection(RequestFrontConnexion request)
        {
            log.Info("api.Connection ...");
            try
            {
                string queryConnexion = nameBdd.connection(request);
                int Count = 0;

                if (this.OpenConnection() == true)
                {
                    //Create Mysql Command
                    MySqlCommand cmd = new MySqlCommand(queryConnexion, connection);

                    //ExecuteScalar will return one value
                    Count = int.Parse(cmd.ExecuteScalar().ToString());

                    //close Connection
                    this.CloseConnection();

                    return Count;
                }
                else
                {
                    return Count;
                }
            }catch(Exception ex)
            {
                log.Error(ex);
                return 0;
            }            
        }

        //RECUP ID ACCOUNT
        public int RetrieveIdAccount(RequestFrontConnexion request)
        {
            log.Info("api.CONNECTION.RetrieveIdAccount ...");

            try
            {
                string queryRecupID = nameBdd.retrieveUserIdConnection(request);
                int id = 0;

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd2 = new MySqlCommand(queryRecupID, connection);
                    MySqlDataReader dataReader = cmd2.ExecuteReader();

                    while (dataReader.Read())
                    {
                        id = nameBdd.retrieveFieldAccountIdConnection(dataReader);
                    }

                    dataReader.Close();

                    this.CloseConnection();

                    return id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 0;
            }
        }

        //INSERT TOKEN
        public void InsertToken(int idAccount, string tokenServer, string tokenClient)
        {
            log.Info("api.CONNECTION.InsertToken ...");

            try
            {
                string queryInsertToken = nameBdd.addToken(idAccount, tokenServer, tokenClient);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryInsertToken, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        //PASSAGE EN LIGNE
        public void NowOnline(int idAccount)
        {
            log.Info("api.CONNECTION.NowOnline ...");

            try
            {
                string queryPassageEnLigne = nameBdd.nowOnline(idAccount);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryPassageEnLigne, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        public Banishment TestIfUserBan(int idAccount)
        {
            log.Info("api.CONNECTION.TestIfUserBan ...");

            try
            {
                Banishment banishment = new Banishment();

                string queryIfBanned = nameBdd.testIfUserBan(idAccount);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryIfBanned, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        banishment.hasBanned = true;
                        banishment.start = nameBdd.banishmentStart(dataReader);
                        banishment.during = nameBdd.banishmentDuring(dataReader);
                        banishment.end = nameBdd.banishmentEnd(dataReader);
                        banishment.reason = nameBdd.banishmentReason(dataReader);
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
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        #endregion

        #region INSCRIPTION 

        public bool Registration(RequestFrontInscription request)
        {
            log.Info("api.INSCRIPTION.Registration ...");

            try
            {
                string queryAddAccount = nameBdd.registrationAddAccount(request);

                string quertRecupIdAccount = nameBdd.registrationRecupIdAccount(request);

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
                        id = int.Parse(dataReader["MAX_ID"].ToString());
                    }

                    dataReader.Close();

                    string queryAddUtilisateur = nameBdd.registrationAddUser(id, request);

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
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool TestIfEmailExist(string email)
        {
            log.Info("api.INSCRIPTION.TestIfEmailExist ...");

            try
            {
                string queryEmailExist = nameBdd.testIfEmailExist(email);

                bool exist = false;

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryEmailExist, connection);


                    if (int.Parse(cmd.ExecuteScalar().ToString()) == 1)
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
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        #endregion

        #region CONFIGURATION
        public List<String> RetrieveListOfForbiddenWord()
        {
            log.Info("api.CONFIGURATION.RetrieveListOfForbiddenWord ...");

            try
            {
                string queryRecupListeMotsInteerdits = nameBdd.retrieveForbiddenWordList();

                List<String> forbiddenWordList = new List<string>();

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd2 = new MySqlCommand(queryRecupListeMotsInteerdits, connection);
                    MySqlDataReader dataReader = cmd2.ExecuteReader();

                    while (dataReader.Read())
                    {
                        forbiddenWordList.Add(nameBdd.fieldForbiddenWord(dataReader));
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
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool InsertForbiddenWord(ForbiddenWord forbiddenWord)
        {
            log.Info("api.CONFIGURATION.InsertForbiddenWord ...");

            try
            {
                string queryAddMotInterdit = nameBdd.insertForbiddenWord(forbiddenWord.word);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryAddMotInterdit, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion

        #region UTILISATEUR
        public int RetrieveUserId(RequestFrontUtilisateur request)
        {
            log.Info("api.UTILISATEUR.RetrieveUserId ...");

            try
            {
                string queryRecupIdUtilisateur = nameBdd.retrieveUserID(request);

                int idAccount = 0;

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryRecupIdUtilisateur, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        idAccount = nameBdd.retrieveUserIdInt(dataReader);
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
            catch (Exception ex)
            {
                log.Error(ex);
                return 0;
            }
        }
        public Boolean disconectionUser(int idAccount)
        {
            log.Info("api.UTILISATEUR.DisconnectionUser ...");
            try
            {
                string queryDisconnectionUser = nameBdd.disconectionUser(idAccount);

                if(this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryDisconnectionUser, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    log.Info("Utilisateur #" + idAccount + " vient de se deconnecter");
                }

                return true;
            }catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public User RetrieveUser(int idAccount)
        {
            log.Info("api.UTILISATEUR.RetrieveUser ...");

            try
            {
                string queryRecuperationUtilisateur = nameBdd.retrieveUser(idAccount);

                User user = new User();

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd2 = new MySqlCommand(queryRecuperationUtilisateur, connection);
                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        user.nickname = nameBdd.retrieveNicknameUser(dataReader2);
                        switch (nameBdd.retrieveStatusUser(dataReader2))
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
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        
        public void BanningUser(DateTime startDate, int during, string reason, int idAccount)
        {
            log.Info("api.UTILISATEUR.BanningUser ...");

            try
            {
                DateTime dateFin = startDate.AddDays(during);
                string queryBannirUtilisateur = nameBdd.banUser(idAccount, startDate, during, dateFin, reason);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryBannirUtilisateur, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
        #endregion

        #region Social
        public List<Friend> RetrieveFriendListDatabase(int idAccount)
        {
            log.Info("api.SOCIAL.RetrieveFriendListDatabase ...");

            try
            {
                string queryRecupIdFriend = nameBdd.retrieveIdAccountFriend(idAccount);


                List<Friend> friendList = new List<Friend>();
                List<int> friendsIdList = new List<int>();

                if (this.OpenConnection() == true)
                {
                    //On récupère les ID des amis
                    MySqlCommand cmd2 = new MySqlCommand(queryRecupIdFriend, connection);
                    MySqlDataReader dataReader = cmd2.ExecuteReader();

                    while (dataReader.Read())
                    {
                        friendsIdList.Add(nameBdd.retrieveIdFriend(dataReader));
                    }

                    dataReader.Close();

                    //On récupère les infos des amis

                    for (int i = 0; i < friendsIdList.Count; i++)
                    {
                        string queryRecupInfosFriends = nameBdd.retrieveInfoFriends(friendsIdList, i);

                        MySqlCommand cmdQueryUser = new MySqlCommand(queryRecupInfosFriends, connection);
                        MySqlDataReader dataReaderUser = cmdQueryUser.ExecuteReader();

                        while (dataReaderUser.Read())
                        {
                            Friend friend = new Friend();

                            friend.nickname = nameBdd.retrieveNicknameFriend(dataReaderUser);
                            friend.status = nameBdd.retrieveStatusFriend(dataReaderUser);

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
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }

        #endregion

        #region GAME LIST
        public List<Game> RetrieveGameList()
        {
            log.Info("api.GAME_LIST.RetrieveGameList ...");

            try
            {
                List<Game> gameList = new List<Game>();


                string queryRetrieveGameList = nameBdd.retrieveGameList();

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryRetrieveGameList, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Game game = new Game();

                        game.id = nameBdd.retrieveIdGame(dataReader);
                        game.title = nameBdd.retrieveTitleGame(dataReader);
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
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }
        public List<Game> RetrieveUserGameList(int idAccount)
        {
            log.Info("api.GAME_LIST.RetrieveUserGameList ...");

            try
            {
                List<Game> userGameList = new List<Game>();
                List<int> userGameIdList = new List<int>();

                string queryRetrieveUserGameList = nameBdd.retrieveGameListUser(idAccount);

                if (this.OpenConnection() == true)
                {
                    //On récupère les ID des amis
                    MySqlCommand cmd2 = new MySqlCommand(queryRetrieveUserGameList, connection);
                    MySqlDataReader dataReader = cmd2.ExecuteReader();

                    while (dataReader.Read())
                    {
                        userGameIdList.Add(nameBdd.retrieveFieldIdGame(dataReader));
                    }

                    dataReader.Close();

                    //On récupère les infos des amis

                    for (int i = 0; i < userGameIdList.Count; i++)
                    {
                        string queryRecupInfoGame = nameBdd.retrieveGameInfos(userGameIdList, i);

                        MySqlCommand cmdQueryUser = new MySqlCommand(queryRecupInfoGame, connection);
                        MySqlDataReader dataReaderUser = cmdQueryUser.ExecuteReader();

                        while (dataReaderUser.Read())
                        {
                            Game game = new Game();

                            game.title = nameBdd.retrieveNameGame(dataReaderUser);

                            userGameList.Add(game);
                        }

                        dataReaderUser.Close();
                    }

                    this.CloseConnection();

                    return userGameList;
                }
                else
                {
                    return userGameList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }
        #endregion

        #region NEWS
        public List<News> RetrieveNewsList()
        {
            log.Info("api.NEWS.RetrieveNewsList ...");

            try
            {
                List<News> newsList = new List<News>();


                string queryRetrieveNewsList = nameBdd.retrieveNewsList();

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryRetrieveNewsList, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        News news = new News();
                        Collection<Object> listeFieldsNews = new Collection<object>();

                        listeFieldsNews = nameBdd.retrieveFieldsNews(dataReader);

                        news.idNews = int.Parse(listeFieldsNews[0].ToString());
                        news.titleNews = listeFieldsNews[1].ToString();
                        news.dateNews = DateTime.Parse(listeFieldsNews[2].ToString()).ToString(CONST.DATE_FORMAT);
                        news.imageNews = listeFieldsNews[3].ToString();
                        news.contentNews = listeFieldsNews[4].ToString();
                        news.categNews = listeFieldsNews[5].ToString();

                        newsList.Add(news);
                    }

                    dataReader.Close();

                    this.CloseConnection();
                    return newsList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion

    }
}
