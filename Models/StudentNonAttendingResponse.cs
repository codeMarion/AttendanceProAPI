using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class StudentNonAttendingResponse
    {
        public int Year { get; set; }
        public int NotAttendingStudents { get; set; }
    }
}
