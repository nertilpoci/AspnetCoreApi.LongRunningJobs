using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTaskSchedule.Services;
using Microsoft.AspNetCore.Mvc;
using ApiTaskSchedule.DB;
namespace ApiTaskSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        IScheduler _scheduler;
        public JobsController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var jobs = new List<ApiTaskSchedule.DB.Job> { new ApiTaskSchedule.DB.Job { Type = Enum.JobType.Default } };
            return Ok(jobs);
        }

    }
}
