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

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            return await courseService.GetAllCourses();
        }

        [HttpGet("{courseCode}")]
        public async Task<IActionResult> GetCourse(string courseCode)
        {
            return await courseService.GetCourse(courseCode);
        }

        [HttpPost("absenceData")]
        public async Task<IActionResult> GetCourseAbsenceReasonData([FromBody] string[] courseCodes)
        {
            return await courseService.GetCourseAbsenceReasonData(courseCodes);
        }

        [HttpPost("coursedata")]
        public async Task<IActionResult> AttendanceDataByCourse([FromBody] string[] courseCodes)
        {
            return await courseService.GetAttendanceDataByCourse(courseCodes);
        }

        [HttpPost("attendanceoverteaching")]
        public async Task<IActionResult> AttendanceDataByTeachingSessions([FromBody] string[] courseCodes)
        {
            return await courseService.AttendanceDataByTeachingSessions(courseCodes);
        }
    }
}
