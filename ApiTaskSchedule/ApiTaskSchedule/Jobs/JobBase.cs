using ApiTaskSchedule.DB;
using ApiTaskSchedule.Enum;
using ApiTaskSchedule.Hubs;
using ApiTaskSchedule.Models;
using ApiTaskSchedule.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Jobs
{
    public interface IJob
    {
         Guid JobId { get; set; }
         JobType Type { get; set; }
         Task Run();
         Task BuildAndRun(Guid ownerId);
    }
    public abstract class JobBase : IJob
    {
        public string Name { get; set; }
        public Guid JobId { get; set; } 
        public Guid OwnerId { get; set; }
        public JobType Type { get; set; } = JobType.Default;
        private IJobPersister _jobPersister;
        public JobBase(IJobPersister jobPersister)
        {
            _jobPersister = jobPersister;
        }
        protected async Task AddOuput(string content)
        {
            await _jobPersister.AddOuput(this.JobId, content);
        }
        protected async Task SetStatus(JobStatus status)
        {
            await _jobPersister.SetStatus(this.JobId, status);
        }
        protected async Task SetPercent(int percent)
        {
            await _jobPersister.SetPercent(this.JobId, percent);
        }
        public async Task<JobBase> Build()
        {
            var job = await _jobPersister.CreateJob(Type, OwnerId, Name,DateTime.UtcNow);
            this.JobId = job.Id;
            return this;
        }

        public abstract  Task Run();

        public async  Task BuildAndRun(Guid ownerId)
        {
            this.OwnerId = ownerId;
            var job = await this.Build();
            await job.Run();
            await _jobPersister.SetEnd(JobId, DateTime.UtcNow);
        }
    }
   
}
