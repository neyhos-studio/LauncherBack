using LauncherBack.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Connexion
{
    public class ResponseFront
    {
        public bool hasError { get; set; }
        public Object error { get; set; } 
        public Object response { get; set; }
    }
}
