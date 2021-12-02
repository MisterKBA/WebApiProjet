using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProjet.Controllers
{
    [Route("testapi")]
    [ApiController]
    public class TestController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("private")]
        [Authorize]
        public IActionResult Private() 
        {
            return Ok(
                new
                {
                    message = "hello from an end point"
                });
        }
    }
}

