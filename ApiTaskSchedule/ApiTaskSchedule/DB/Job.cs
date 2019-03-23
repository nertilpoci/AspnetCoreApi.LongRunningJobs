using ApiTaskSchedule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.DB
{
    public partial class Jobs
    {
        public Jobs()
        {
            JobOutputs = new HashSet<JobOutputs>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public JobStatus Status { get; set; }
        public int PercentCompleted { get; set; }
        public JobType Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ICollection<JobOutputs> JobOutputs { get; set; }
    }
}
