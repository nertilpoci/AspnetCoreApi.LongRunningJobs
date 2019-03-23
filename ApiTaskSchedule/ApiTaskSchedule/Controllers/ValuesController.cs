using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTaskSchedule.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiTaskSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IScheduler _scheduler;
        public ValuesController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

    }
}
