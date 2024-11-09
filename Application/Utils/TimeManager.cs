using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class TimeManager
    {
        public static TimeManager Instance { get; } = new TimeManager();

        public TimeOnly GetCurrentTimeOnly()
        {
            return TimeOnly.FromDateTime(DateTime.Now);
        }
    }
}
