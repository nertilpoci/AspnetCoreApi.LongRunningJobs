using ApiTaskSchedule.DB;
using ApiTaskSchedule.Enum;
using ApiTaskSchedule.Hubs;
using ApiTaskSchedule.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Services
{
    public interface IJobPersister
    {
        Task<DB.Jobs> CreateJob(JobType type, Guid ownerId, string name,string description, DateTime? start);
        Task AddOuput(Guid jobId, string content);
        Task SetStatus(Guid jobId, JobStatus status);
        Task SetPercent(Guid jobId, int percent);
        Task SetEnd(Guid jobId, DateTime? time);
        Task SetStart(Guid jobId, DateTime? time);
    }
    public class JobPersister: IJobPersister
    {
        private readonly TaskSchedulerDbContext _db;
        private readonly IHubContext<JobHub> _hubContext;
        public JobPersister(TaskSchedulerDbContext db, IHubContext<JobHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }
     
     
       
        public async Task AddOuput(Guid jobId, string content)
        {
            await _db.JobOutputs.AddAsync(new JobOutputs { Id = Guid.NewGuid(), Time=DateTime.UtcNow, Content = content, JobId = jobId });
            await _db.SaveChangesAsync();
            var outputNotification = new JobUpdateNotification<JobOutputs> { Type = Enum.JobNotificationType.OutputUpdate, JobId = jobId, Data = new JobOutputs { Content = content, Time = DateTime.UtcNow } };
            await _hubContext.Clients.All.SendAsync("onJobInfo", outputNotification);
        }
        public async Task SetStart(Guid jobId, DateTime? time)
        {
            var job = await GetJob(jobId);
            job.StartTime = time;
            await _db.SaveChangesAsync();
        }
        public async Task SetEnd(Guid jobId, DateTime? time)
        {
            var job = await GetJob(jobId);
            job.EndTime = time;
            await _db.SaveChangesAsync();
            var updateNotification = new JobUpdateNotification<DateTime?> { Type = Enum.JobNotificationType.EndTimeUpdate, JobId = jobId, Data = time };
            await _hubContext.Clients.All.SendAsync("onJobInfo", updateNotification);
        }
        public async Task SetStatus(Guid jobId, JobStatus status)
        {
            var job = await GetJob(jobId);
            if (job == null) return; 
            job.Status = status;
            await _db.SaveChangesAsync();
            var statusUpdate = new JobUpdateNotification<int> { Type = Enum.JobNotificationType.StatusUpdate, JobId = jobId, Data = (int)status };
            await _hubContext.Clients.All.SendAsync("onJobInfo", statusUpdate);
        }
        public async Task SetPercent(Guid jobId, int percent)
        {
            var job = await GetJob(jobId);
            if (job == null) return;
            job.PercentCompleted = percent;
            await _db.SaveChangesAsync();
            var percentangeNotification = new JobUpdateNotification<int> { Type = Enum.JobNotificationType.PercentUpdate, JobId = jobId, Data = percent };
            await _hubContext.Clients.All.SendAsync("onJobInfo", percentangeNotification);
        }
        public async Task<DB.Jobs> CreateJob(JobType type, Guid ownerId, string name, string description, DateTime? start)
        {
            var job = new DB.Jobs { Name= name,Description= description, OwnerId = ownerId, Type = type, Status = JobStatus.Created, PercentCompleted=0, StartTime=start };
            var entity =await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();
            var newJob = entity.Entity;
            newJob.JobOutputs = Enumerable.Empty<JobOutputs>().ToList();
            var newJobNotificaiton = new JobUpdateNotification<ApiTaskSchedule.DB.Jobs> { Type = Enum.JobNotificationType.Created, Data = newJob };
            await _hubContext.Clients.All.SendAsync("onJobInfo", newJobNotificaiton);
            return entity.Entity;
        }
        private async Task<DB.Jobs> GetJob(Guid id)
        {
            return await _db.Jobs.FindAsync(id);
           
        }
    }
}
