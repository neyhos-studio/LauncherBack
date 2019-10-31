using LauncherBack.Controllers.Games;
using LauncherBack.Controllers.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Utilisateur
{
    public class User
    {
        public string nickname { get; set; }
        public string ip { get; set; }
        public int status { get; set; }
        public string token { get; set; }
        public string avatar { get; set; }
        public List<Friend> friendList { get; set; }
        public List<Game> gameList { get; set; }
    }
}
