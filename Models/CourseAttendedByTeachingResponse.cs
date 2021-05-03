using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class CourseAttendedByTeachingResponse
    {
        public string Course { get; set; }
        public List<CourseAttendedByTeachingData> AttendanceData { get; set; }
    }
}