using System;
using System.Collections.Generic;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Controllers.Inscription;
using LauncherBack.Controllers.Utilisateur;
using MySql.Data.MySqlClient;
using MSG = LauncherBack.Helpers.Messages;
using CONST = LauncherBack.Helpers.Constantes;


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
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + login + ";" + "PASSWORD=" + password + ";";

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
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
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
        public int Connexion(RequestFrontConnexion request)
        {
            string query = "SELECT COUNT(*) FROM NS_ACCOUNTS as ACC WHERE ACC.ACCOUNT_EMAIL = '"+request.email+"' AND ACC.ACCOUNT_PASSWORD = '"+request.password+"'";
            int Count = 0;

            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

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
        public int RecupIdAccount(RequestFrontConnexion request)
        {
            string queryRecupID = "SELECT ACCOUNT_ID FROM NS_ACCOUNTS as ACC WHERE ACC.ACCOUNT_EMAIL = '" + request.email + "' AND ACC.ACCOUNT_PASSWORD = '" + request.password + "'";
            int id = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryRecupID, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                while (dataReader.Read())
                {
                    id = int.Parse(dataReader["ACCOUNT_ID"] + "");
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
            string queryRecupID = "INSERT INTO NS_TOKENS (TOKEN_ACCOUNT_ID, TOKEN_TOKEN_SERVER, TOKEN_TOKEN_CLIENT, TOKEN_DATE_CREATION) VALUES (" + idAccount+", '" + tokenServer+"', '"+ tokenClient + "', '"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryRecupID, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            else
            {
            }
        }

        //PASSAGE EN LIGNE
        public void PassageEnLigne(int idAccount)
        {
            string passageEnLigne = "UPDATE NS_UTILISATEURS SET UTILISATEUR_ETAT = 2 WHERE UTILISATEUR_ID_ACCOUNT = "+ idAccount+"";

            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = passageEnLigne;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
            else
            {
            }
        }

        #endregion

        #region INSCRIPTION 

        public bool Inscription(RequestFrontInscription request)
        {
            string queryAddAccount = "INSERT INTO NS_ACCOUNTS (ACCOUNT_EMAIL, ACCOUNT_PASSWORD) VALUES ('"+request.email+"', '"+request.password+"')";
            string quertRecupIdAccount = "SELECT MAX(ACCOUNT_ID) as MAX_ID FROM NS_ACCOUNTS WHERE ACCOUNT_EMAIL = '" + request.email + "'";
            int id = 0;
            bool estInscrit = false;

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

                string queryAddUtilisateur = "INSERT INTO NS_UTILISATEURS (UTILISATEUR_ID_ACCOUNT, UTILISATEUR_PSEUDO) VALUES (" + id + ", '" + request.pseudo + "')";
                MySqlCommand cmd2 = new MySqlCommand(queryAddUtilisateur, connection);
                cmd2.ExecuteNonQuery();

                this.CloseConnection();

                return estInscrit = true;

            }
            else
            {
                return estInscrit;
            }
        }

        public bool EmailExist(string email)
        {
            string query = "SELECT COUNT(*) FROM NS_ACCOUNTS as ACC WHERE ACC.ACCOUNT_EMAIL = '" + email + "'";

            bool exist = false;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);


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
        public List<String> RecupListeMotsInterdits()
        {
            string queryRecupListe = "SELECT * FROM NS_MOTS_INTERDITS";
            List<String> listeMotsInterdits = new List<string>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryRecupListe, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                while (dataReader.Read())
                {
                    listeMotsInterdits.Add(dataReader["MOT_INTERDIT_LIBELLE"].ToString());
                }

                dataReader.Close();

                this.CloseConnection();

                return listeMotsInterdits;
            }
            else
            {
                return null;
            }


        }
        public bool InsertMotInterdit(string motInterdit)
        {
            string queryAddMotInterdit = "INSERT INTO NS_MOTS_INTERDITS (MOT_INTERDIT_LIBELLE) VALUES ('" + motInterdit + "')";

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
        public int RecupIdUtilisateur(RequestFrontUtilisateur request)
        {
            string query = "SELECT * FROM NS_TOKENS WHERE TOKEN_TOKEN_CLIENT = '"+request.token+"'";
            Utilisateur utilisateur = new Utilisateur();
            int idAccount = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    idAccount = int.Parse(dataReader["TOKEN_ACCOUNT_ID"] + "");
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

        public Utilisateur RecupUtilisateur(int idAccount)
        {
            string queryUser = "SELECT * FROM NS_UTILISATEURS WHERE UTILISATEUR_ID_ACCOUNT = " + idAccount + "";
            Utilisateur utilisateur = new Utilisateur();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd2 = new MySqlCommand(queryUser, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                while (dataReader2.Read())
                {
                    utilisateur.pseudo = dataReader2["UTILISATEUR_PSEUDO"] + "";
                    switch (dataReader2["UTILISATEUR_ETAT"])
                    {
                        case 1:
                            utilisateur.status = CONST.HORS_LIGNE;
                            break;
                        case 2:
                            utilisateur.status = CONST.EN_LIGNE;
                            break;
                        case 3:
                            utilisateur.status = CONST.ABSENT;
                            break;
                        case 4:
                            utilisateur.status = CONST.OCCUPE;
                            break;
                        case 5:
                            utilisateur.status = CONST.INVISIBLE;
                            break;
                        default:
                            break;
                    }
                }

                dataReader2.Close();
                this.CloseConnection();

                return utilisateur;
            } else
            {
                return null;
            }
            }
        #endregion
    }
}
