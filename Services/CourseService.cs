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
    public class CourseService : ICourseService
    {
        private DataContext context;
        public CourseService(DataContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// This method is used to get course code and title from all courses.
        /// </summary>
        public async Task<IActionResult> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            IEnumerable<List<FileRow>> dbQuery = context.Students.AsEnumerable().GroupBy(x => x.CourseCode).ToList().Select(x => x.ToList());
            foreach(List<FileRow> course in dbQuery)
            {
                courses.Add(new Course 
                { 
                    CourseCode = course.FirstOrDefault().CourseCode,
                    CourseTitle = course.FirstOrDefault().CourseTitle
                });
            }
            return new OkObjectResult(courses);
        }

        /// <summary>
        /// This method is used to get information about a particular course.
        /// </summary>
        public async Task<IActionResult> GetCourse(string courseCode)
        {
            FileRow course = context.Students.Where(x => x.CourseCode == courseCode).FirstOrDefault();
            return new OkObjectResult(course);
        }
        /// <summary>
        /// This method is used to get course absence reasons data.
        /// </summary>
        public async Task<IActionResult> GetCourseAbsenceReasonData(string[] courseCodes)
        {
            List<AbsenceReasonByYear> AbsencesByYear = new List<AbsenceReasonByYear>();
            AbsenceReason overallAbsenceReason = new AbsenceReason();
            List<FileRow> courses;
            if (courseCodes.Count() == 0)
            {
                courses = context.Students.ToList();
            }
            else
            {
                courses = context.Students.Where(x => courseCodes.Contains(x.CourseCode)).ToList();
            }
            //Remodelling data for charts in the front-end
            foreach(FileRow course in courses)
            {
                AbsenceReasonByYear year = AbsencesByYear.Where(x => x.Year == int.Parse(course.CourseYear)).Select(x => x).FirstOrDefault();
                if (year == null)
                {
                    AbsencesByYear.Add(new AbsenceReasonByYear() 
                    { 
                        Year = int.Parse(course.CourseYear),
                        Attended = course.Attended,
                        Explained = course.Explained,
                        NonAttended = course.NonAttended,
                        Teaching = course.Teaching
                    });
                }
                else
                {
                    year.Attended += course.Attended;
                    year.Explained += course.Explained;
                    year.NonAttended += course.NonAttended;
                    year.Teaching += course.Teaching;
                }
                overallAbsenceReason.Teaching += course.Teaching;
                overallAbsenceReason.Attended += course.Attended;
                overallAbsenceReason.Explained += course.Explained;
                overallAbsenceReason.NonAttended += course.NonAttended;
            }
            AbsencesByYear = AbsencesByYear.OrderBy(x => x.Year).ToList();
            AbsenceReasonResponse absenceReasonResponse = new AbsenceReasonResponse 
            { 
                Overall=overallAbsenceReason,
                AbsenceReasons=AbsencesByYear
            };
            return new OkObjectResult(absenceReasonResponse);
        }
        /// <summary>
        /// This method is used to get attedendance data by course
        /// </summary>
        public async Task<IActionResult> GetAttendanceDataByCourse(string[] courseCodes)
        {
            List<AttendanceByPeriod> AttendanceByPeriodResponse = new List<AttendanceByPeriod>();
            List<FileRow> courses = context.Students.Where(x => courseCodes.Contains(x.CourseCode)).ToList();
            IEnumerable<IEnumerable<FileRow>> GroupedRows = courses.GroupBy(x => x.UserId).Select(x => x.Select(x => x));
            //Remodelling data to suitable format
            foreach(IEnumerable<FileRow> Row in GroupedRows)
            {
                try
                {
                    AttendanceByPeriod CurrentCourse = AttendanceByPeriodResponse.Where(x => x.Course == Row.FirstOrDefault().CourseCode).Select(x => x).FirstOrDefault();
                    if(CurrentCourse == null)
                    {
                        AttendanceByPeriod NewCourse = new AttendanceByPeriod
                        {
                            Course = Row.FirstOrDefault().CourseCode,
                            Attended = new List<int>(new int[Row.Count()])
                        };
                        AttendanceByPeriodResponse.Add(NewCourse);
                        for(int i = 0; i < Row.Count(); i++)
                        {
                            NewCourse.Attended[i] += Row.ElementAt(i).Attended;
                        }
                    }
                    else
                    {
                        for(int i = 0; i < Row.Count(); i++)
                        {
                            CurrentCourse.Attended[i] += Row.ElementAt(i).Attended;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error occured");
                }
            }
            return new OkObjectResult(AttendanceByPeriodResponse);
        }
        /// <summary>
        /// This method is used to get attedendance data by teaching sessions
        /// </summary>
        public async Task<IActionResult> AttendanceDataByTeachingSessions(string[] courseCodes)
        {
            List<CourseAttendedByTeachingResponse> attendanceData = new List<CourseAttendedByTeachingResponse>();
            List<FileRow> dbData = context.Students.Where(x => courseCodes.Contains(x.CourseCode)).ToList();
            IEnumerable<List<FileRow>> students = dbData.AsEnumerable().GroupBy(x => x.UserId).Select(x => new List<FileRow>(x));
            //Remodelling data to a suitable format
            foreach(List<FileRow> student in students)
            {
                CourseAttendedByTeachingResponse course = attendanceData.Where(x => x.Course == student.FirstOrDefault().CourseCode).FirstOrDefault();
                CourseAttendedByTeachingData courseData = course != null ? course.AttendanceData.Where(x => x.UserId == student.FirstOrDefault().UserId).FirstOrDefault() : null;
                if (course == null)
                {
                    course = new CourseAttendedByTeachingResponse
                    {
                        Course = student.FirstOrDefault().CourseCode,
                        AttendanceData = new List<CourseAttendedByTeachingData>()
                    };
                    attendanceData.Add(course);  
                }
                if (courseData == null)
                {
                    courseData = new CourseAttendedByTeachingData
                    {
                        UserId = student.FirstOrDefault().UserId,
                        Attended = 0,
                        Teaching = 0
                    };
                    course.AttendanceData.Add(courseData);
                }
                foreach (FileRow data in student)
                {
                    courseData.Attended += data.Attended;
                    courseData.Teaching += data.Teaching;
                }
            }
            return new OkObjectResult(attendanceData);
        }
    }


}
