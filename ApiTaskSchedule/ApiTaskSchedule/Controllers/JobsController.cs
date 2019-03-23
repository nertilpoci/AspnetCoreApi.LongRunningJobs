using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTaskSchedule.Services;
using Microsoft.AspNetCore.Mvc;
using ApiTaskSchedule.DB;
using ApiTaskSchedule.Hubs;
using Microsoft.AspNetCore.SignalR;
using ApiTaskSchedule.Models;
using ApiTaskSchedule.Enum;
using ApiTaskSchedule.Jobs;

namespace ApiTaskSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        IScheduler _scheduler;
        IHubContext<JobHub> _hubContext;
        static Guid jobId = Guid.NewGuid();
        public JobsController(IScheduler scheduler, IHubContext<JobHub> hubContext)
        {
            _scheduler = scheduler;
            _hubContext = hubContext;
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_scheduler.Jobs(z=>z.Status!= JobStatus.Finished));
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
             _scheduler.Schedule<StartEngineJob>();
            return Ok();
        }
        

    }
}
