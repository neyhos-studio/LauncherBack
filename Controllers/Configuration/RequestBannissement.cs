using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Configuration
{
    public class RequestBannissement
    {
        public int idAccount { get; set; }
        public DateTime dateDebut { get; set; }
        public int duree { get; set; }
        public string motif { get; set; }
    }
}
