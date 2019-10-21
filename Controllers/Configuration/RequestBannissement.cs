using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Configuration
{
    public class RequestBannissement
    {
        public int idAccount { get; set; }
        public DateTime startDate { get; set; }
        public int duration { get; set; }
        public string reason { get; set; }
    }
}
