using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class TimeSingleton
    {
        private TimeSingleton()
        {
            
        }
        public static TimeOnly GetCurrentTimeOnly()
        {
            return TimeOnly.FromDateTime(DateTime.Now);
        }
    }
}
