using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceProAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private ICourseService courseService;
        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        /// <summary>
        /// Retrieves all courses
        /// </summary>
        /// <returns>List of Courses</returns>
        /// <response code="200">Returns all courses</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            return await courseService.GetAllCourses();
        }
        /// <summary>
        /// Retrieves a course
        /// </summary>
        /// <param name="courseCode">Course code</param>
        /// <returns>Returns a course</returns>
        [HttpGet("{courseCode}")]
        public async Task<IActionResult> GetCourse(string courseCode)
        {
            return await courseService.GetCourse(courseCode);
        }
        /// <summary>
        /// Retrieves Absence Reason data
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <returns></returns>
        /// <response code="200">Returns absence reason data</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("absenceData")]
        public async Task<IActionResult> GetCourseAbsenceReasonData([FromBody] string[] courseCodes)
        {
            return await courseService.GetCourseAbsenceReasonData(courseCodes);
        }
        /// <summary>
        /// Retrieves attendance data by courses
        /// </summary>
        /// <param name="courseCodes">String array of course code</param>
        /// <returns>Returns attendance data by courses</returns>
        /// <response code="200">Returns attendance data by courses</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("coursedata")]
        public async Task<IActionResult> AttendanceDataByCourse([FromBody] string[] courseCodes)
        {
            return await courseService.GetAttendanceDataByCourse(courseCodes);
        }
        /// <summary>
        /// Retrieves the attendance data with associated teaching sessions
        /// </summary>
        /// <param name="courseCodes">String array of course codes</param>
        /// <returns>Retrieves the attendance data with associated teaching sessions</returns>
        /// <response code="200">Returns the attendance data with associated teaching sessions</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("attendanceoverteaching")]
        public async Task<IActionResult> AttendanceDataByTeachingSessions([FromBody] string[] courseCodes)
        {
            return await courseService.AttendanceDataByTeachingSessions(courseCodes);
        }
    }
}
