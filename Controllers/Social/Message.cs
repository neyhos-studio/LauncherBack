using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Social
{
    public class Message
    {
        public int messageId { get; set; }
        public DateTime messageDateTime { get; set; }
        public String message { get; set; }
        public int messageFrom { get; set; }
        public int messageTo { get; set; }
    }
}
