using ApiTaskSchedule.Hubs;
using ApiTaskSchedule.Services;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class StartEngineJob : JobBase<EngineJobData>
    {
        public StartEngineJob (IJobPersister jobPersister ) : base(jobPersister) {
           
        }

        public async override Task Run(EngineJobData data)
        {
            
            await SetStatus(Enum.JobStatus.Running);
            await Task.Delay(10000);
            await SetPercent(10);
            await AddOuput("Hang on i am doing some cool stuff here");
            await Task.Delay(5000);
            await AddOuput("Just a couple of more seconds");
            await Task.Delay(80000);
            await SetPercent(50);
            await AddOuput("Almost finished");
            await Task.Delay(5000);
            await AddOuput("Ok it's done");
            await SetPercent(100);
            await SetStatus(Enum.JobStatus.Finished);
        }
    }

    public class EngineJobData : IJobData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
