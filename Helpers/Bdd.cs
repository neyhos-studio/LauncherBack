using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using MySql.Data.MySqlClient;

namespace LauncherBack.Helpers
{
    public class Bdd
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public Bdd()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "92.222.80.11";
            database = "NEYHOS_STUDIO";
            uid = "NEYHOS_STUDIO";
            password = "0knqtalmp1";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
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

        //ACCOUNT EXIST
        public bool Connexion(RequestFront request)
        {
            string query = "SELECT COUNT(*) FROM NS_ACCOUNTS as ACC WHERE ACC.ACCOUNT_EMAIL = '"+request.accountEmail+"' AND ACC.ACCOUNT_PASSWORD = '"+request.accountPassword+"'";

            bool exist = false;

            if(this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
               

                if (int.Parse(cmd.ExecuteScalar() + "") == 1)
                {
                    exist = true;
                }

                this.CloseConnection();
            } else
            {
                Console.WriteLine("Erreur de connexion à la BDD");
            }

            return exist;
        }

        //RECUP ID ACCOUNT
        public int RecupIdAccount(RequestFront request)
        {
            string queryRecupID = "SELECT ACCOUNT_ID FROM NS_ACCOUNTS as ACC WHERE ACC.ACCOUNT_EMAIL = '" + request.accountEmail + "' AND ACC.ACCOUNT_PASSWORD = '" + request.accountPassword + "'";
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
            string passageEnLigne = "UPDATE NS_UTILISATEURS SET UTILISATEUR_ETAT = 2 WHERE UTILISATEUR_ID_ACCOUNT = 2";

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
    }
}
