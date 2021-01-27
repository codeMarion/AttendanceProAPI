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

        [HttpGet("count")]
        public IActionResult GetStudentCountWithoutFiltering()
        {
            return studentService.GetStudentCount("");
        }

        [HttpGet("count/{searchTerm}")]
        public IActionResult GetStudentCountWithFiltering(string searchTerm)
        {
            return studentService.GetStudentCount(searchTerm);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            return  studentService.GetStudent(id);
        }

        [HttpGet("page/{page}")]
        public IActionResult GetStudentsPageWithoutFiltering(int page)
        {
            return studentService.GetStudentsPage(page, "");
        }

        [HttpGet("page/{page}/{searchTerm}")]
        public IActionResult GetStudentsPageWithFiltering(int page, string searchTerm)
        {
            return studentService.GetStudentsPage(page, searchTerm);
        }
    }
}
