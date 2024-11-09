using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class DateManager
    {
        public static DateManager Instance { get; } = new DateManager();

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

    }
}
