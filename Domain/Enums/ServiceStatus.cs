using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ServiceStatus
    {
        Assigned,
        InProgress,
        Accepted,
        Rejected,
        Reassigned,
        Reached,
        Started,
        Completed
    }
}
