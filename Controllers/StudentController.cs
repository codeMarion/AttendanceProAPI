using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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
        [AllowAnonymous]
        [HttpGet("persistentAbsence")]
        public IActionResult GetPersistentAbsenteesDataCount()
        {
            return studentService.GetPersistentAbsenteesDataCount();
        }
        [AllowAnonymous]
        [HttpGet("persistentAbsence/{page}")]
        public IActionResult GetPersistentAbsenteesData(int page)
        {
            return studentService.GetPersistentAbsenteesData(page);
        }
        [AllowAnonymous]
        [HttpGet("persistentAbsenteesCountByYear")]
        public IActionResult GetPersistentAbsenteesCountByYear()
        {
            return studentService.GetPersistentAbsenteesCountByYear();
        }
        [AllowAnonymous]
        [HttpGet("persistentAbsenteesCountByCourse")]
        public IActionResult GetPersistentAbsenteesCountByCourse()
        {
            return studentService.GetPersistentAbsenteesCountByCourse();
        }
        [AllowAnonymous]
        [HttpGet("nonAttendingCount")]
        public IActionResult GetNonAttendingStudentsCount()
        {
            return studentService.GetNonAttendingStudentsCount();
        }
        [AllowAnonymous]
        [HttpGet("nonAttendingStudents/{page}")]
        public IActionResult GetNonAttendingStudents(int page)
        {
            return studentService.GetNonAttendingStudents(page);
        }

        [AllowAnonymous]
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
    }
}
