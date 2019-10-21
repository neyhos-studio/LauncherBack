using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Utilisateur
{
    public class Banishment
    {
        public bool hasBanned { get; set; }
        public DateTime start { get; set; }
        public int during { get; set; }
        public DateTime end { get; set; }
        public string endFormalize { get; set; }
        public string reason { get; set; }
    }
}
