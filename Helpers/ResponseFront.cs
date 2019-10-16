using System;

namespace LauncherBack.Controllers.Connexion
{
    public class ResponseFront
    {
        public bool hasError { get; set; }
        public Object error { get; set; } 
        public Object response { get; set; }
    }
}
