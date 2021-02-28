using AttendanceProAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface IStudentService
    {
        IActionResult GetStudentCount(string[] courseCodes, string searchTerm);
        IActionResult GetStudent(int studentId);
        IActionResult UpdateStudent(PersonalDetails details);
        IActionResult GetStudentsPage(string[] courseCodes, int pageNo, string searchTerm);
        IActionResult GetPersistentAbsenteesDataCount();
        IActionResult GetPersistentAbsenteesData(int page);
        IActionResult GetPersistentAbsenteesCountByYear();
        IActionResult GetPersistentAbsenteesCountByCourse();
        IActionResult GetNonAttendingStudentsCount();
        IActionResult GetNonAttendingStudents(int page);
        IActionResult GetNonAttendingCountByYear();
        IActionResult GetAttendanceDataByPeriod();
        IActionResult GetAverageAttendance();
    }
}
