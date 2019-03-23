using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTaskSchedule.Enum
{
    public enum JobNotificationType
    {
       Created=1,
       StatusUpdate=2,
       OutputUpdate=3,
       PercentUpdate=4,
       EndTimeUpdate=5
    }
}
