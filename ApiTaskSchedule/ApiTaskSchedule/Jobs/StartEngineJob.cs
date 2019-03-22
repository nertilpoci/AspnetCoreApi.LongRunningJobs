using ApiTaskSchedule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Job
{
    public class StartEngineJob: JobBase
    {
        public StartEngineJob (IJobPersister jobPersister) : base(jobPersister) {}

        public async override Task Run()
        {
            await SetStatus(Enum.JobStatus.Running);
            await Task.Delay(10000);

            await AddOuput("Hang on i am doing some cool stuff here");
            await Task.Delay(5000);
            await AddOuput("Just a couple of more seconds");
            await Task.Delay(5000);
            await AddOuput("Almost finished");
            await Task.Delay(5000);
            await AddOuput("Ok it's done");

            await SetStatus(Enum.JobStatus.Finished);
        }
    }
}
