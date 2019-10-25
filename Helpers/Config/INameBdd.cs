using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Inscription;
using LauncherBack.Controllers.Utilisateur;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LauncherBack.Helpers.Config
{
    interface INameBdd
    {
        string retrieveUserID(RequestFrontUtilisateur token);
        int retrieveUserIdInt(MySqlDataReader dataReader);
        string connection(RequestFrontConnexion request);
        string retrieveUserIdConnection(RequestFrontConnexion request);
        int retrieveFieldAccountIdConnection(MySqlDataReader dataReader);
        string addToken(int idAccount, string tokenServer, string tokenClient);
        string nowOnline(int idAccount);
        string testIfUserBan(int idAccount);
        DateTime banishmentStart(MySqlDataReader dataReader);
        int banishmentDuring(MySqlDataReader dataReader);
        DateTime banishmentEnd(MySqlDataReader dataReader);
        string banishmentReason(MySqlDataReader dataReader);
        string registrationAddAccount(RequestFrontInscription request);
        string registrationRecupIdAccount(RequestFrontInscription request);
        string registrationAddUser(int idAccount, RequestFrontInscription request);
        string testIfEmailExist(string email);
        string retrieveForbiddenWordList();
        string fieldForbiddenWord(MySqlDataReader dataReader);
        string insertForbiddenWord(string motInterdit);
        string retrieveUser(int idAccount);
        string retrieveNicknameUser(MySqlDataReader dataReader);
        int retrieveStatusUser(MySqlDataReader dataReader);
        string banUser(int idAccount, DateTime startDate, int during, DateTime dateFin, string reason);
        string retrieveIdAccountFriend(int idAccount);
        int retrieveIdFriend(MySqlDataReader dataReader);
        string retrieveInfoFriends(List<int> friendListId, int i);
        string retrieveNicknameFriend(MySqlDataReader dataReader);
        string retrieveStatusFriend(MySqlDataReader dataReader);
        string retrieveGameListUser(int idAccount);
        int retrieveFieldIdGame(MySqlDataReader dataReader);
        string retrieveGameInfos(List<int> userGameList, int i);
        string retrieveNameGame(MySqlDataReader dataReader);
        string retrieveGameList();
        int retrieveIdGame(MySqlDataReader dataReader);
        string retrieveTitleGame(MySqlDataReader dataReader);
        string retrieveNewsList();
        Collection<Object> retrieveFieldsNews(MySqlDataReader dataReader);
        string disconectionUser(int idAccount);
    }
}
