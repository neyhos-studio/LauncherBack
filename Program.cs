using System.IO;
using System.Reflection;
using System.Xml;
using LauncherBack.Helpers;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

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
            log.Info("Démarrage de l'API ...");
            log.Info("Connexion à la BDD ...");
            Bdd bdd = new Bdd();
            if (bdd.OpenConnection())
            {
                log.Info("BDD connectée !");
                CreateWebHostBuilder(args).Build().Run();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(config => {
                    config.ClearProviders();
                })
                .UseStartup<Startup>();

    }
}
