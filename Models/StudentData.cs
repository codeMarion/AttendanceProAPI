using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class StudentData
    {
        public string CourseYear { get; set; }
        public string RegStatus { get; set; }
        public int Teaching { get; set; }
        public int Attended { get; set; }
        public int Explained { get; set; }
        public int NonAttended { get; set; }
        public float AttendancePercentage { get; set; }
        public float UnexcusedAttendancePercentage { get; set; }
        public string LastAttendance { get; set; }
    }
}
