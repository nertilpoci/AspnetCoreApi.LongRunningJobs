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


        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<JobOutputs> JobOutputs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<JobOutputs>(entity =>
            {
                entity.HasIndex(e => e.JobId);

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Time).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobOutputs)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_JobOutputs");
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });


        }
    }

}
