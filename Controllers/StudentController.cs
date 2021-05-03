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
        /// <summary>
        /// Retrieves the number of students
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <returns>Returns the number of students</returns>
        /// <response code="200">Returns the number of students</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("count")]
        public IActionResult GetStudentCountWithoutFiltering([FromBody] string[] courseCodes)
        {
            return studentService.GetStudentCount(courseCodes,"");
        }
        /// <summary>
        /// Retrieves the number of students with filtering options
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <param name="searchTerm">Student id keyword</param>
        /// <returns>Returns the number of students with filtering options</returns>
        /// <response code="200">Returns the number of students with filtering options</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("count/{searchTerm}")]
        public IActionResult GetStudentCountWithFiltering([FromBody]string[] courseCodes, string searchTerm)
        {
            return studentService.GetStudentCount(courseCodes, searchTerm);
        }
        /// <summary>
        /// Retrieves a student object
        /// </summary>
        /// <param name="id">Student id</param>
        /// <returns>Returns a student object</returns>
        /// <response code="200">Returns a student object</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            return studentService.GetStudent(id);
        }
        /// <summary>
        /// Updates student personal detils
        /// </summary>
        /// <param name="details">Updated personal details object</param>
        /// <returns>Returns updated student object</returns>
        /// <response code="200">Student personal details updated successfully</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("update")]
        public IActionResult UpdateStudent([FromBody] PersonalDetails details)
        {
            return studentService.UpdateStudent(details);
        }
        /// <summary>
        /// Retrieve array of students objects for a given page
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <returns>Returns array of students objects for a given page</returns>
        /// <response code="200">Student personal details updated successfully</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("page/{page}")]
        public IActionResult GetStudentsPageWithoutFiltering([FromBody] string[] courseCodes, int page)
        {
            return studentService.GetStudentsPage(courseCodes, page, "");
        }
        /// <summary>
        /// Retrieve array of students objects for a given page with filtering options
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <param name="searchTerm">Student id keyword</param>
        /// <returns>Returns array of students objects for a given page with filtering options</returns>
        /// <response code="200">Student personal details updated successfully</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("page/{page}/{searchTerm}")]
        public IActionResult GetStudentsPageWithFiltering([FromBody] string[] courseCodes, int page, string searchTerm)
        {
            return studentService.GetStudentsPage(courseCodes, page, searchTerm);
        }
        /// <summary>
        /// Retrieve the number of persistent absentees 
        /// </summary>
        /// <param name="margin">Risk student percentage</param>
        /// <returns>Returns the number of persistent absentees </returns>
        /// <response code="200">The number of persistent absentees</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("persistentAbsenceCount/{margin}")]
        public IActionResult GetPersistentAbsenteesDataCount(double margin)
        {
            return studentService.GetPersistentAbsenteesDataCount(margin);
        }
        /// <summary>
        /// Retrieve persistent absentees by page
        /// </summary>
        /// <param name="margin">Risk student percentage</param>
        /// <param name="page">Page number</param>
        /// <returns>Returns persistent absentees by page</returns>
        /// <response code="200">Persistent absentees by page</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("persistentAbsence/{margin}/{page}")]
        public IActionResult GetPersistentAbsenteesData(double margin, int page)
        {
            return studentService.GetPersistentAbsenteesData(margin, page);
        }
        /// <summary>
        /// Retrieve the number of persistent absentees by year
        /// </summary>
        /// <param name="margin">Risk student percentage</param>
        /// <returns>Returns the number of persistent absentees by year</returns>
        /// <response code="200">The number of persistent absentees by year</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("persistentAbsenteesCountByYear/{margin}")]
        public IActionResult GetPersistentAbsenteesCountByYear(double margin)
        {
            return studentService.GetPersistentAbsenteesCountByYear(margin);
        }
        /// <summary>
        /// Retrieve the number of persistent absentees by course
        /// </summary>
        /// <param name="margin">Risk student percentage</param>
        /// <returns>Returns the number of persistent absentees by course</returns>
        /// <response code="200">The number of persistent absentees by course</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("persistentAbsenteesCountByCourse/{margin}")]
        public IActionResult GetPersistentAbsenteesCountByCourse(double margin)
        {
            return studentService.GetPersistentAbsenteesCountByCourse(margin);
        }
        /// <summary>
        /// Retrieve the number of not attending students 
        /// </summary>
        /// <returns>Returns the number of not attending students</returns>
        /// <response code="200">The number of not attending students</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("nonAttendingCount")]
        public IActionResult GetNonAttendingStudentsCount()
        {
            return studentService.GetNonAttendingStudentsCount();
        }
        /// <summary>
        /// Retrieve not attending students by page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>Returns not attending students by page</returns>
        /// <response code="200">Retrieve not attending students by page</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("nonAttendingStudents/{page}")]
        public IActionResult GetNonAttendingStudents(int page)
        {
            return studentService.GetNonAttendingStudents(page);
        }
        /// <summary>
        /// Retrieve not attending students by year
        /// </summary>
        /// <returns>Returns not attending students by year</returns>
        /// <response code="200">Retrieve not attending students by year</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("nonAttendingStudentsByYear")]
        public IActionResult GetNonAttendingStudentsByYear()
        {
            return studentService.GetNonAttendingCountByYear();
        }
        /// <summary>
        /// Retrieve attendance data by period
        /// </summary>
        /// <returns>Returns attendance data by period</returns>
        /// <response code="200">Retrieve attendance data by period</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("attendanceByPeriod")]
        public IActionResult GetAttendanceDataByPeriod()
        {
            return studentService.GetAttendanceDataByPeriod();
        }
        /// <summary>
        /// Retrieve average attendance
        /// </summary>
        /// <returns>Returns average attendance</returns>
        /// <response code="200">Retrieve average attendance</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("avgAttendance")]
        public IActionResult GetAverageAttendance()
        {
            return studentService.GetAverageAttendance();
        }
        /// <summary>
        /// Retrieve tracked students 
        /// </summary>
        /// <returns>Returns tracked students</returns>
        /// <response code="200">Retrieve tracked students</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("getTrackedStudents")]
        public IActionResult GetTrackedStudents([FromBody] int [] students)
        {
            return studentService.GetTrackedStudents(students);
        }
    }
}
