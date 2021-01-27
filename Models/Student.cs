using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class Student
    {
        public int UserId { get; set; }
        public string StudyLevel { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
    }
}
