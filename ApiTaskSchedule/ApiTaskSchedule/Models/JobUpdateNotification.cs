using ApiTaskSchedule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Models
{
    public class JobUpdateNotification<T>
    {
        public JobNotificationType Type { get; set; }
        public Guid? JobId { get; set; }
        public T Data { get; set; }
    }
}
