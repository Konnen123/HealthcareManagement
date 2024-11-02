﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class GuidChecker
    {
        public static bool BeAValidGuid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
