using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.DB
{
    public class Job
    {
        public Job()
        {
            JobOutputs = new HashSet<JobOutput>();
        }
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public int Status { get; set; }
        public int PercentCompleted { get; set; }
        public int Type { get; set; }
        public IEnumerable<JobOutput> JobOutputs { get; set; }
    }
}
