using AttendanceProAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IActionResult> GetAllCourses();
        Task<IActionResult> GetCourse(string courseCode);
        Task<IActionResult> GetCourseAbsenceReasonData(string[] courseCodes);
        Task<IActionResult> GetAttendanceDataByCourse(string[] courseCodes);
        Task<IActionResult> AttendanceDataByTeachingSessions(string[] courseCodes);
    }
}
