using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class StudentPageResponse
    {
        public int UserId { get; set; }
        public string StudyLevel { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public string RegStatus { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<StudentData> StudentData { get; set; }
    }
}
