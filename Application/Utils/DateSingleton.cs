﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class DateSingleton
    {
        protected DateSingleton()
        {
            
        }
        public static DateOnly GetCurrentDateOnly()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

    }
}