using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class FileRow
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("user")]
        public int UserId { get; set; }
        [JsonProperty("studyLevel")]
        public string StudyLevel { get; set; }
        [JsonProperty("courseYear")]
        public string CourseYear { get; set; }
        [JsonProperty("regStatus")]
        public string RegStatus { get; set; }
        [JsonProperty("courseTitle")]
        public string CourseTitle { get; set; }
        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }
        [JsonProperty("teaching")]
        public int Teaching { get; set; }
        [JsonProperty("attended")]
        public int Attended { get; set; }
        [JsonProperty("explained")]
        public int Explained { get; set; }
        [JsonProperty("nonAttended")]
        public int NonAttended { get; set; }
        [JsonProperty("percentAttendance")]
        public float AttendancePercentage { get; set; }
        [JsonProperty("unexcusedPercentAttendance")]
        public float UnexcusedAttendancePercentage { get; set; }
        [JsonProperty("lastAttendance")]
        public string LastAttendance { get; set; }
    }
}
