using ApiTaskSchedule.DB;
using ApiTaskSchedule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Services
{
    public interface IJobPersister
    {
        Task<DB.Job> CreateJob(JobType type, Guid ownerId);
        Task AddOuput(Guid jobId, string content);
        Task SetStatus(Guid jobId, JobStatus status);
        Task SetPercent(Guid jobId, int percent);
    }
    public class JobPersister: IJobPersister
    {
        private readonly TaskSchedulerDbContext _db;
        public JobPersister(TaskSchedulerDbContext db)
        {
            _db = db;
        }
        
        public async Task AddOuput(Guid jobId, string content)
        {
            _db.JobOutputs.Add(new JobOutput { Id = Guid.NewGuid(), Time=DateTime.UtcNow, Content = content, JobId = jobId });
            await _db.SaveChangesAsync();
        }
        public async Task SetStatus(Guid jobId, JobStatus status)
        {
            var job = await GetJob(jobId);
            if (job == null) return; 
            job.Status = status;
            await _db.SaveChangesAsync();
        }
        public async Task SetPercent(Guid jobId, int percent)
        {
            var job = await GetJob(jobId);
            if (job == null) return;
            job.PercentCompleted = percent;
            await _db.SaveChangesAsync();
        }
        public async Task<DB.Job> CreateJob(JobType type, Guid ownerId)
        {
            var job = new DB.Job { OwnerId = ownerId, Type = type, Status = JobStatus.Created, PercentCompleted=0 };
            var entity =await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }
        private async Task<DB.Job> GetJob(Guid id)
        {
            return await _db.Jobs.FindAsync(id);
           
        }
    }
}
