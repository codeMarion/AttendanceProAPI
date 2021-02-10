using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class CourseAttendedByTeachingData
    {
        public int UserId { get; set; }
        public int Teaching { get; set; }
        public int Attended { get; set; }
    }
}
