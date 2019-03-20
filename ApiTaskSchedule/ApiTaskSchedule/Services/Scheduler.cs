using ApiTaskSchedule.DB;
using ApiTaskSchedule.Job;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Services
{
    public interface IScheduler
    {
        void Schedule<T>() where T : IJob;
    }
    public class Scheduler : IScheduler
    {
        private readonly TaskSchedulerDbContext _db;
        public Scheduler(TaskSchedulerDbContext db)
        {
            _db = db;
        }

        public void Schedule<T>() where T: IJob
        {
            BackgroundJob.Enqueue<T>(  j => j.BuildAndRun(Guid.NewGuid()));
        }

    }
}
