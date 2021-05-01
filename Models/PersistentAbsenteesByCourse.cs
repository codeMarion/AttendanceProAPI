using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class PersistentAbsenteesByCourse
    {
        public string Course { get; set; }
        public int Students { get; set; }
    }
}
