using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.DB
{
    public partial class JobOutputs
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public Jobs Job { get; set; }
    }
}
