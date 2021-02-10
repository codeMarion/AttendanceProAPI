using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class AttendanceByPeriod
    {
        public string Course { get; set; }
        public List<int> Attended { get; set; }
    }
}
