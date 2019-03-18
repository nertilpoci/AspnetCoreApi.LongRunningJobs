using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.DB
{
   
    public partial class TaskSchedulerDbContext : DbContext
    {
    
    public TaskSchedulerDbContext(DbContextOptions<TaskSchedulerDbContext> options)
       : base(options)
    {
    }
    public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobOutput> JobOutputs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });
            modelBuilder.Entity<JobOutput>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobOutputs)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_JobOutputs");
            });
            
        }
    }

}
