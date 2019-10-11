using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            uid = "root";
            password = "Projet_meuporg57";
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

        //SELECT
        public bool Select()
        {
            string query = "SELECT * FROM NS_ETATS_UTILISATEUR";

            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            if(this.OpenConnection() == true)
            {
                return this.OpenConnection();
                /*MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ETAT_UTILISATEUR_ID"] + "");
                    list[1].Add(dataReader["ETAT_UTILISATEUR_LIBELLE"] + "");
                    list[2].Add(dataReader["ETAT_UTILISATEUR_COLOR"] + "");
                }

                dataReader.Close();
                this.CloseConnection();
                return list;*/
            } else
            {
                return this.OpenConnection();
                //return list;
            }
        }
    }
}
