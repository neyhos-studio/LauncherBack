using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Utilisateur
{
    public class Bannissement
    {
        public bool hasBanned { get; set; }
        public DateTime debut { get; set; }
        public int duree { get; set; }
        public DateTime fin { get; set; }
        public string finForm { get; set; }
        public string raison { get; set; }
    }
}
