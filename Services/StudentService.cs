using AttendanceProAPI.Data;
using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services
{
    public class StudentService : IStudentService
    {
        private DataContext DbContext;
        public StudentService(DataContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public IActionResult GetStudent(int studentId)
        {
            List<FileRow> dbQuery = DbContext.Students.Where(x => x.UserId == studentId).ToList();
            List<StudentPageResponse> student = GetStudentResponse(dbQuery);
            return new OkObjectResult(student);
        }

        public IActionResult GetStudentCount(string searchTerm)
        {
            if (searchTerm == "")
            {
                int count = DbContext.Students.Select(x => x.UserId).Distinct().Count();
                return new OkObjectResult(count);
            }
            else
            {
                int count = DbContext.Students.Where(x => x.UserId.ToString().Contains(searchTerm)).Select(x => x.UserId).Distinct().Count();
                return new OkObjectResult(count);
            }
        }

        public IActionResult GetStudentsPage(int pageNo, string searchTerm)
        {
            if (searchTerm == "")
            {
                IEnumerable<List<FileRow>> dbQuery = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                List<Student> response = GetStudentsPageResponse(dbQuery.Skip((pageNo - 1) * 12).Take(12));
                return new OkObjectResult(response);
            }
            else
            {
                IEnumerable<List<FileRow>> dbQuery = DbContext.Students.AsEnumerable().Where(x => x.UserId.ToString().Contains(searchTerm)).GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                List<Student> response = GetStudentsPageResponse(dbQuery.Skip((pageNo - 1) * 12).Take(12));
                return new OkObjectResult(response);
            }
        }

        private List<StudentPageResponse> GetStudentResponse(List<FileRow> dbQuery)
        {
            List<StudentPageResponse> response = new List<StudentPageResponse>();
            List<StudentData> studentData = new List<StudentData>();
            foreach (FileRow record in dbQuery)
            {
                studentData.Add(new StudentData()
                {
                    RegStatus = record.RegStatus,
                    CourseYear = record.CourseYear,
                    Attended = record.Attended,
                    Explained = record.Explained,
                    Teaching = record.Teaching,
                    NonAttended = record.NonAttended,
                    AttendancePercentage = record.AttendancePercentage,
                    UnexcusedAttendancePercentage = record.UnexcusedAttendancePercentage,
                    LastAttendance = record.LastAttendance
                });
            }
            response.Add(new StudentPageResponse()
            {
                UserId = dbQuery.Select(x => x.UserId).FirstOrDefault(),
                StudyLevel = dbQuery.Select(x => x.StudyLevel).FirstOrDefault(),
                CourseTitle = dbQuery.Select(x => x.CourseTitle).FirstOrDefault(),
                CourseCode = dbQuery.Select(x => x.CourseCode).FirstOrDefault(),
                StudentData = studentData
            });
            return response;
        }

        private List<Student> GetStudentsPageResponse(IEnumerable<List<FileRow>> dbQuery)
        {
            List<Student> response = new List<Student>();
            foreach (List<FileRow> studentRecord in dbQuery)
            {
                response.Add(new Student()
                {
                    UserId = studentRecord.Select(x => x.UserId).FirstOrDefault(),
                    StudyLevel = studentRecord.Select(x => x.StudyLevel).FirstOrDefault(),
                    CourseTitle = studentRecord.Select(x => x.CourseTitle).FirstOrDefault(),
                    CourseCode = studentRecord.Select(x => x.CourseCode).FirstOrDefault()
                });
            }
            return response;
        }
    }
}
