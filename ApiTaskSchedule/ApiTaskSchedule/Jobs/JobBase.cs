using ApiTaskSchedule.Enum;
using ApiTaskSchedule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Job
{
    public interface IJob
    {
         Guid JobId { get; set; }
         JobType Type { get; set; }
         Task<JobBase> Build();
         Task Run();
        Task BuildAndRun(Guid ownerId);
    }
    public abstract class JobBase : IJob
    {
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
            var job = await _jobPersister.CreateJob(Type, OwnerId);
            this.JobId = job.Id;
            return this;
        }

        public abstract  Task Run();

        public async  Task BuildAndRun(Guid ownerId)
        {
            this.OwnerId = ownerId;
            await (await this.Build()).Run();
        }
    }
   
}
