using System;
using System.Collections.Generic;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Inscription;
using LauncherBack.Controllers.Utilisateur;
using MySql.Data.MySqlClient;
using MSG = LauncherBack.Helpers.Messages;
using CONST = LauncherBack.Helpers.Constantes;
using LauncherBack.Controllers.Social;
using LauncherBack.Controllers.Games;
using LauncherBack.Helpers.Config;
using LauncherBack.Controllers.News;
using System.Collections.ObjectModel;
using LauncherBack.Controllers.Configuration;
using System.Diagnostics;

namespace LauncherBack.Helpers
{
    public class Bdd
    {
        private MySqlConnection connection;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Stopwatch stopwatch = new Stopwatch();

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
            stopwatch.Start();
            
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

                    stopwatch.Stop();
                    log.Info("api.CONNECTION.RetrieveIdAccount ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
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
            stopwatch.Start();

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

            stopwatch.Stop();
            log.Info("api.CONNECTION.InsertToken ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

        }

        //PASSAGE EN LIGNE
        public void NowOnline(int idAccount)
        {
            stopwatch.Start();

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

            stopwatch.Stop();
            log.Info("api.CONNECTION.NowOnline ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
        }

        public Banishment TestIfUserBan(int idAccount)
        {
            stopwatch.Start();
            

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

                    stopwatch.Stop();
                    log.Info("api.CONNECTION.TestIfUserBan ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
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
            stopwatch.Start();

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

                    stopwatch.Stop();
                    log.Info("api.INSCRIPTION.Registration ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();

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

                stopwatch.Stop();
                log.Info("api.INSCRIPTION.TestIfEmailExist ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();

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

                    stopwatch.Stop();
                    log.Info("api.CONFIGURATION.RetrieveListOfForbiddenWord ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();
            

            try
            {
                string queryAddMotInterdit = nameBdd.insertForbiddenWord(forbiddenWord.word);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryAddMotInterdit, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();

                    stopwatch.Stop();
                    log.Info("api.CONFIGURATION.InsertForbiddenWord ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
        public void cleanDatabase()
        {
            stopwatch.Start();
            List<string> listRequest = nameBdd.cleanDatabase();

            if (this.OpenConnection() == true)
            {
                for (int i = 0; i < listRequest.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand(listRequest[i], connection);
                    cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
            }

            stopwatch.Stop();
            log.Info("Database clean ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
        }
        public void cleanTokens()
        {
            stopwatch.Start();
            List<string> listRequestTokens = nameBdd.cleanTokens();

            if(this.OpenConnection() == true)
            {
                for(int i = 0; i<listRequestTokens.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand(listRequestTokens[i], connection);
                    cmd.ExecuteNonQuery();
                }

                this.CloseConnection();
            }

            stopwatch.Stop();
            log.Info("Table tokens clean ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
        }
        public void disconnectAllUsers()
        {
            stopwatch.Start();
            string requestDisconnectAllUsers = nameBdd.disconnectAllUsers();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(requestDisconnectAllUsers, connection);
                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }

            stopwatch.Stop();
            log.Info("All users have been logged out ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
        }
        #endregion

        #region UTILISATEUR
        public int RetrieveUserId(RequestFrontUtilisateur request)
        {
            stopwatch.Start();
            
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

                    stopwatch.Stop();
                    log.Info("api.UTILISATEUR.RetrieveUserId ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
        public Boolean disconectionUser(int idAccount, string token)
        {
            stopwatch.Start();

            try
            {
                string queryDisconnectionUser = nameBdd.disconectionUser(idAccount);
                string queryDeleteTokenUser = nameBdd.deleteTokenUser(token);

                if(this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryDisconnectionUser, connection);
                    cmd.ExecuteNonQuery();
                    MySqlCommand cmd2 = new MySqlCommand(queryDeleteTokenUser, connection);
                    cmd2.ExecuteNonQuery();
                    this.CloseConnection();
                    log.Info("Utilisateur #" + idAccount + " vient de se deconnecter");
                    log.Info("Token du compte #" + idAccount + " supprimé");
                }

                stopwatch.Stop();
                log.Info("api.UTILISATEUR.DisconnectionUser ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

                return true;
            }catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public User RetrieveUser(int idAccount)
        {
            stopwatch.Start();

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
                        user.avatar = nameBdd.retrieveAvatarUser(dataReader2);
                        user.status = nameBdd.retrieveStatusUser(dataReader2);

                    }

                    dataReader2.Close();

                    this.CloseConnection();

                    stopwatch.Stop();
                    log.Info("api.UTILISATEUR.RetrieveUser ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();

            try
            {
                DateTime dateFin = startDate.AddDays(during);
                string queryBannirUtilisateur = nameBdd.banUser(idAccount, startDate, during, dateFin, reason);
                string queryNowBannedUser = nameBdd.nowBannedUser(idAccount);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryBannirUtilisateur, connection);
                    cmd.ExecuteNonQuery();
                    MySqlCommand cmd2 = new MySqlCommand(queryNowBannedUser, connection);
                    cmd2.ExecuteNonQuery();
                    this.CloseConnection();
                }

                stopwatch.Stop();
                log.Info("api.UTILISATEUR.BanningUser ... " + stopwatch.Elapsed.TotalSeconds + " ms)");
                log.Info("Utilisateur #"+idAccount+" banni pendant "+during+" jour(s)");
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
            stopwatch.Start();
            

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
                            friend.avatar =  nameBdd.retrieveAvatarFriend(dataReaderUser);


                            friendList.Add(friend);
                        }

                        dataReaderUser.Close();
                    }

                    this.CloseConnection();

                    stopwatch.Stop();
                    log.Info("api.SOCIAL.RetrieveFriendListDatabase ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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

        public void sendMessage(Message requestMessage)
        {
            stopwatch.Start();

            try
            {
                string queryInsertMessage = nameBdd.sendMessage(requestMessage);

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(queryInsertMessage, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }

                stopwatch.Stop();
                log.Info("api.SOCIAL.SendMessage ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            

        }

        #endregion

        #region GAME LIST
        public List<Game> RetrieveGameList()
        {
            stopwatch.Start();

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

                    stopwatch.Stop();
                    log.Info("api.GAME_LIST.RetrieveGameList ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();

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

                    stopwatch.Stop();
                    log.Info("api.GAME_LIST.RetrieveUserGameList ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");

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
            stopwatch.Start();
            

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

                    stopwatch.Stop();
                    log.Info("api.NEWS.RetrieveNewsList ... (" + stopwatch.Elapsed.TotalSeconds + " ms)");
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
