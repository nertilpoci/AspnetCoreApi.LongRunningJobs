using ApiTaskSchedule.DB;
using ApiTaskSchedule.Jobs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Services
{
    public interface IScheduler
    {
        void Schedule<TJobType, TData>(TData input) where TJobType : IJob<TData> where TData : IJobData, new();
        IEnumerable<DB.Jobs> Jobs(Expression<Func<DB.Jobs, bool>> query);
    }
    public class Scheduler : IScheduler
    {
        private readonly TaskSchedulerDbContext _db;
        public Scheduler(TaskSchedulerDbContext db)
        {
            _db = db;
        }
        public IEnumerable<DB.Jobs> Jobs(Expression<Func<DB.Jobs, bool>> query)
        {
            if(query==null) return _db.Jobs.Include(z => z.JobOutputs).ToList();
            return _db.Jobs.Where(query).Include(z => z.JobOutputs).ToList();
        }
        public void Schedule<TJobType, TData>(TData input) where TJobType : IJob<TData> where TData : IJobData, new()
        {
            BackgroundJob.Enqueue<TJobType>(j => j.BuildAndRun(input));
        }

    }
}
