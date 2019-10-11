
using LauncherBack.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LauncherBack.Controllers.Connexion
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnexionController : ControllerBase
    {
        Bdd bdd = new Bdd();

        [HttpPost]
        [ActionName("Inscription")]
        public void Connexion([FromBody] RequestFront request)
        {
            Console.WriteLine(":::" + bdd.Select());
        }
    }
}
