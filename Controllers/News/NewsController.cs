using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using LauncherBack.Helpers;
using log4net;
using Microsoft.AspNetCore.Mvc;
using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;

namespace LauncherBack.Controllers.News
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("RetrieveNewsList")]
        public ResponseFront RetrieveNewsList()
        {
            Bdd bdd = DataBaseConnection.databaseConnection();
            List<News> newsList = new List<News>();

            newsList = bdd.RetrieveNewsList();
            responseFront.response = newsList;

            return responseFront;
        }
    }
}
