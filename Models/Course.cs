using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class Course
    {
        [JsonProperty("courseTitle")]
        public string CourseTitle { get; set; }
        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }
    }
}
