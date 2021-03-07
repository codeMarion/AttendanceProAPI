using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceProAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService studentService;
        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpPost("count")]
        public IActionResult GetStudentCountWithoutFiltering([FromBody] string[] courseCodes)
        {
            return studentService.GetStudentCount(courseCodes,"");
        }

        [HttpPost("count/{searchTerm}")]
        public IActionResult GetStudentCountWithFiltering([FromBody]string[] courseCodes, string searchTerm)
        {
            return studentService.GetStudentCount(courseCodes, searchTerm);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            return studentService.GetStudent(id);
        }

        [HttpPost("update")]
        public IActionResult UpdateStudent([FromBody] PersonalDetails details)
        {
            return studentService.UpdateStudent(details);
        }
        
        [HttpPost("page/{page}")]
        public IActionResult GetStudentsPageWithoutFiltering([FromBody] string[] courseCodes, int page)
        {
            return studentService.GetStudentsPage(courseCodes, page, "");
        }

        [HttpPost("page/{page}/{searchTerm}")]
        public IActionResult GetStudentsPageWithFiltering([FromBody] string[] courseCodes, int page, string searchTerm)
        {
            return studentService.GetStudentsPage(courseCodes, page, searchTerm);
        }

        [HttpGet("persistentAbsenceCount/{margin}")]
        public IActionResult GetPersistentAbsenteesDataCount(double margin)
        {
            return studentService.GetPersistentAbsenteesDataCount(margin);
        }

        [HttpGet("persistentAbsence/{margin}/{page}")]
        public IActionResult GetPersistentAbsenteesData(double margin, int page)
        {
            return studentService.GetPersistentAbsenteesData(margin, page);
        }

        [HttpGet("persistentAbsenteesCountByYear/{margin}")]
        public IActionResult GetPersistentAbsenteesCountByYear(double margin)
        {
            return studentService.GetPersistentAbsenteesCountByYear(margin);
        }

        [HttpGet("persistentAbsenteesCountByCourse/{margin}")]
        public IActionResult GetPersistentAbsenteesCountByCourse(double margin)
        {
            return studentService.GetPersistentAbsenteesCountByCourse(margin);
        }

        [HttpGet("nonAttendingCount")]
        public IActionResult GetNonAttendingStudentsCount()
        {
            return studentService.GetNonAttendingStudentsCount();
        }

        [HttpGet("nonAttendingStudents/{page}")]
        public IActionResult GetNonAttendingStudents(int page)
        {
            return studentService.GetNonAttendingStudents(page);
        }


        [HttpGet("nonAttendingStudentsByYear")]
        public IActionResult GetNonAttendingStudentsByYear()
        {
            return studentService.GetNonAttendingCountByYear();
        }

        [HttpGet("attendanceByPeriod")]
        public IActionResult GetAttendanceDataByPeriod()
        {
            return studentService.GetAttendanceDataByPeriod();
        }
        
        [HttpGet("avgAttendance")]
        public IActionResult GetAverageAttendance()
        {
            return studentService.GetAverageAttendance();
        }

        [HttpPost("getTrackedStudents")]
        public IActionResult GetTrackedStudents([FromBody] int [] students)
        {
            return studentService.GetTrackedStudents(students);
        }
    }
}
