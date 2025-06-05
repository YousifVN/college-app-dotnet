using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeApp.MyLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoLoggerController : ControllerBase
    {
        private readonly IMyLogger _myLogger;

        public DemoLoggerController(IMyLogger myLogger)
        {
            _myLogger = myLogger;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            _myLogger.Log("Logging from index method");
            return Ok();
        }
    }
}
