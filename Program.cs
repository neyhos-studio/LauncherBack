using System;
using System.IO;
using System.Reflection;
using System.Xml;
using LauncherBack.Helpers;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using CRED = LauncherBack.Helpers.Config.Credentials;
using CONST = LauncherBack.Helpers.Constantes;
using test = LauncherBack.Helpers.Config.NameBdd;

[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace LauncherBack
{

    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), log4netConfig["log4net"]);

            string env;
            Console.WriteLine("Sur quelle base de données se connecter ?");
            Console.WriteLine("1 : SERVER");
            Console.WriteLine("2 : LOCAL");
            env = Console.ReadLine();

            if (env == "1")
            {

                CONST.envTravail = 0;

                log.Info("Travail sur environnement de PROD");
                Bdd bdd = new Bdd(CRED.SERVER_PROD, CRED.DATABASE_PROD, CRED.LOGIN_PROD, CRED.PASSWORD_PROD);

                log.Info("Connexion à la BDD de production ...");

                if (bdd.OpenConnection())
                {
                    log.Info("BDD connectée !");
                    log.Info("Démarrage de l'API ...");
                    CreateWebHostBuilder(args).Build().Run();
                }
            } else
            {

                CONST.envTravail = 1;

                log.Info("Travail sur environnement de DEV");
                Bdd bdd = new Bdd(CRED.SERVER_DEV, CRED.DATABASE_DEV, CRED.LOGIN_DEV, CRED.PASSWORD_DEV);
                log.Info("Connexion à la BDD localhost ...");

                if (bdd.OpenConnection())
                {
                    log.Info("BDD connectée !");
                    log.Info("Démarrage de l'API ...");
                    CreateWebHostBuilder(args).Build().Run();
                }
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(config => {
                    config.ClearProviders();
                    //config.AddConsole();
                })
                .UseStartup<Startup>();

    }
}
