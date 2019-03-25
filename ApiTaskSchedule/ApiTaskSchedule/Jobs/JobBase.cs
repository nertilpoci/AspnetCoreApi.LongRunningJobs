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
    public interface IJob<T> where T : IJobData, new()
    {
         Guid JobId { get; set; }
         JobType Type { get; set; }
        Task Run(T input);
        Task<JobBase<T>> Build(T input);
        Task BuildAndRun(T input);
    }
    public interface IJobData
    {
        string Name { get; set; }
        string Description { get; set; }
        Guid OwnerId { get; set; }
    }
    public abstract class JobBase<T> : IJob<T> where T : IJobData, new()
    {
        public Guid JobId { get; set; } 
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
        public async Task<JobBase<T>> Build(T input)
        {

            var job = await _jobPersister.CreateJob(Type,input.OwnerId, input.Name, input.Description, DateTime.UtcNow);
            this.JobId = job.Id;
            return this;
        }

        public abstract  Task Run(T input);

        public async  Task BuildAndRun(T input)
        {
            var job = await this.Build(input);
            await job.Run(input);
            await _jobPersister.SetEnd(JobId, DateTime.UtcNow);
        }
    }
   
}
