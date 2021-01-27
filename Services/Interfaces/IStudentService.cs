using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface IStudentService
    {
        IActionResult GetStudentCount(string searchTerm);
        IActionResult GetStudent(int studentId);
        IActionResult GetStudentsPage(int pageNo, string searchTerm);
    }
}
