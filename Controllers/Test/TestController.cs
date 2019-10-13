using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LauncherBack.Controllers.Connexion;
using Microsoft.AspNetCore.Mvc;
using CONST = LauncherBack.Helpers.Constantes;
using MSG = LauncherBack.Helpers.Messages;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LauncherBack.Controllers.Test
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        ResponseFront responseFront = new ResponseFront();

        [HttpPost]
        [ActionName("Tests")]
        public ResponseFront Tests([FromBody] string email)
        {
            if (!email.Contains("@"))
            {
                responseFront.hasError = true;
                responseFront.error = MSG.INSCRIPTION_EMAIL_INVALID;
                return responseFront;
            }
            else
            {
                responseFront.hasError = false;
                responseFront.response = "C'est OK";
                return responseFront;
            }
        }
    }
}
