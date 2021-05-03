using AttendanceProAPI.Data;
using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceProAPI.Services
{
    public class StudentService : IStudentService
    {
        private DataContext DbContext;
        public StudentService(DataContext DbContext)
        {
            this.DbContext = DbContext;
        }
        /// <summary>
        /// This method is used to retrieve a student object based on the student id.
        /// </summary>
        public IActionResult GetStudent(int studentId)
        {
            List<FileRow> dbQuery = DbContext.Students.Where(x => x.UserId == studentId).ToList();
            StudentPageResponse student = GetStudentResponse(dbQuery);
            PersonalDetails studentPersonalDetails = DbContext.PersonalDetails.Where(x => x.UserId == studentId).FirstOrDefault();
            if (studentPersonalDetails != null)
            {
                student.Phone = studentPersonalDetails.Phone;
                student.Email = studentPersonalDetails.Email;
            }
            return new OkObjectResult(student);
        }
        /// <summary>
        /// This method is used to update personal details of a given student.
        /// </summary>
        public IActionResult UpdateStudent(PersonalDetails details)
        {
            PersonalDetails studentInformation = DbContext.PersonalDetails.Where(x => x.UserId == details.UserId).FirstOrDefault();
            if(studentInformation == null)
            {
                DbContext.PersonalDetails.Add(new PersonalDetails 
                { 
                    UserId=details.UserId,
                    Email=details.Email,
                    Phone=details.Phone
                });
            }
            else
            {
                studentInformation.Phone = details.Phone;
                studentInformation.Email = details.Email;
            }
            DbContext.SaveChanges();
            return new OkResult();
        }
        /// <summary>
        /// This method is used to get the number of students in the database.
        /// </summary>
        public IActionResult GetStudentCount(string[] courseCodes, string searchTerm)
        {
            int count;
            if (searchTerm == "")
            {
                if(courseCodes.Count() > 0)
                    count = DbContext.Students.Where(x => courseCodes.Contains(x.CourseCode)).Select(x => x.UserId).Distinct().Count();
                else
                    count = DbContext.Students.Select(x => x.UserId).Distinct().Count();
                return new OkObjectResult(count);
            }
            else
            {
                if (courseCodes.Count() > 0)
                    count = DbContext.Students.Where(x => x.UserId.ToString().Contains(searchTerm)).Where(x => courseCodes.Contains(x.CourseCode)).Select(x => x.UserId).Distinct().Count();
                else
                    count = DbContext.Students.Where(x => x.UserId.ToString().Contains(searchTerm)).Select(x => x.UserId).Distinct().Count();
                return new OkObjectResult(count);
            }
        }
        /// <summary>
        /// This method is used to retrieve the data about students for a given page.
        /// </summary>
        public IActionResult GetStudentsPage(string[] courseCodes, int pageNo, string searchTerm)
        {
            IEnumerable<List<FileRow>> dbQuery;
            if (searchTerm == "")
            {
                if(courseCodes.Count() > 0)
                     dbQuery = DbContext.Students.Where(x => courseCodes.Contains(x.CourseCode)).AsEnumerable().GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                else
                    dbQuery = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                List<Student> response = GetStudentsPageResponse(dbQuery.Skip((pageNo - 1) * 12).Take(12));
                return new OkObjectResult(response);
            }
            else
            {
                if(courseCodes.Count() > 0)
                {
                    dbQuery = DbContext.Students.AsEnumerable().Where(x => x.UserId.ToString().Contains(searchTerm)).Where(x => courseCodes.Contains(x.CourseCode)).GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                }
                else
                {
                    dbQuery = DbContext.Students.AsEnumerable().Where(x => x.UserId.ToString().Contains(searchTerm)).GroupBy(x => x.UserId).ToList().Select(x => x.ToList());
                }
                List<Student> response = GetStudentsPageResponse(dbQuery.Skip((pageNo - 1) * 12).Take(12));
                return new OkObjectResult(response);
            }
        }
        /// <summary>
        /// This method is used retrieve the number of persistent absentees by dynamic risk level margin.
        /// </summary>
        public IActionResult GetPersistentAbsenteesDataCount(double margin)
        {
            int count = 0;
            var students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            foreach (var student in students)
            {
                int attended = 0;
                int teaching = 0;
                foreach (var row in student)
                {
                    attended += row.Attended;
                    teaching += row.Teaching;
                }
                float avgAttendance =(float)attended / teaching;
                if (avgAttendance > 0 && avgAttendance < margin)
                    count++;
            }
            return new OkObjectResult(count);
        }
        /// <summary>
        /// This method is used to retrieve persistent absentees data by dynamic risk level margin for a given page.
        /// </summary>
        public IActionResult GetPersistentAbsenteesData(double margin, int page)
        {
            List<FileRow> notAttendingStudents = new List<FileRow>();
            var students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            //Remodelling data to a suitable format.
            foreach (var student in students)
            {
                FileRow avgStudent = new FileRow();
                avgStudent.Id = student.FirstOrDefault().Id;
                avgStudent.UserId = student.FirstOrDefault().UserId;
                avgStudent.StudyLevel = student.FirstOrDefault().StudyLevel;
                avgStudent.CourseYear = student.FirstOrDefault().CourseYear;
                avgStudent.RegStatus = student.FirstOrDefault().RegStatus;
                avgStudent.CourseTitle = student.FirstOrDefault().CourseTitle;
                avgStudent.CourseCode = student.FirstOrDefault().CourseCode;
                foreach (var row in student)
                {
                    avgStudent.Teaching += row.Teaching;
                    avgStudent.Attended += row.Attended;
                    avgStudent.Explained += row.Explained;
                    avgStudent.NonAttended += row.NonAttended;
                }
                avgStudent.AttendancePercentage = (float)avgStudent.Attended / avgStudent.Teaching;
                if (avgStudent.AttendancePercentage > 0 && avgStudent.AttendancePercentage < 0.8)
                {
                    notAttendingStudents.Add(avgStudent);
                }
            }
            notAttendingStudents = notAttendingStudents.OrderBy(x => x.AttendancePercentage).ToList();
            notAttendingStudents = notAttendingStudents.Skip((page - 1) * 5).Take(5).ToList();
            return new OkObjectResult(notAttendingStudents);
        }
        /// <summary>
        /// This method is used to retreive the number of not attending students.
        /// </summary>
        public IActionResult GetNonAttendingStudentsCount()
        {
            int count = 0;
            var students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            foreach(var student in students)
            {
                double avgAttendance = 0;
                foreach(var row in student)
                {
                    avgAttendance += row.AttendancePercentage;
                }
                if (avgAttendance == 0)
                    count++;
            }
            return new OkObjectResult(count);
        }
        /// <summary>
        /// This method is used to retrieve not attending students for a given page.
        /// </summary>
        public IActionResult GetNonAttendingStudents(int page)
        {
            List<FileRow> notAttendingStudents = new List<FileRow>();
            var students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            //Remodelling data to a suitable format
            foreach (var student in students)
            {
                FileRow avgStudent = new FileRow();
                avgStudent.Id = student.FirstOrDefault().Id;
                avgStudent.UserId = student.FirstOrDefault().UserId;
                avgStudent.StudyLevel = student.FirstOrDefault().StudyLevel;
                avgStudent.CourseYear = student.FirstOrDefault().CourseYear;
                avgStudent.RegStatus = student.FirstOrDefault().RegStatus;
                avgStudent.CourseTitle = student.FirstOrDefault().CourseTitle;
                avgStudent.CourseCode = student.FirstOrDefault().CourseCode;
                foreach (var row in student)
                {
                    avgStudent.Teaching += row.Teaching;
                    avgStudent.Attended += row.Attended;
                    avgStudent.Explained += row.Explained;
                    avgStudent.NonAttended += row.NonAttended;
                }
                avgStudent.AttendancePercentage = (float)avgStudent.Attended / avgStudent.Teaching;
                if (avgStudent.AttendancePercentage == 0)
                    notAttendingStudents.Add(avgStudent);
            }
            notAttendingStudents = notAttendingStudents.Skip((page - 1) * 5).Take(5).ToList();
            return new OkObjectResult(notAttendingStudents);
        }
        /// <summary>
        /// This method is used retreive the number of persistent absentees by year with a dynamic risk level margin.
        /// </summary>
        public IActionResult GetPersistentAbsenteesCountByYear(double margin)
        {
            List<FileRow> persistentAbsentees = new List<FileRow>();
            IEnumerable<List<FileRow>> students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            //Remodelling data to a suitable format
            foreach (var student in students)
            {
                FileRow avgStudent = new FileRow();
                avgStudent.Id = student.FirstOrDefault().Id;
                avgStudent.UserId = student.FirstOrDefault().UserId;
                avgStudent.StudyLevel = student.FirstOrDefault().StudyLevel;
                avgStudent.CourseYear = student.FirstOrDefault().CourseYear;
                avgStudent.RegStatus = student.FirstOrDefault().RegStatus;
                avgStudent.CourseTitle = student.FirstOrDefault().CourseTitle;
                avgStudent.CourseCode = student.FirstOrDefault().CourseCode;
                foreach (var row in student)
                {
                    avgStudent.Teaching += row.Teaching;
                    avgStudent.Attended += row.Attended;
                    avgStudent.Explained += row.Explained;
                    avgStudent.NonAttended += row.NonAttended;
                }
                avgStudent.AttendancePercentage = (float)avgStudent.Attended / avgStudent.Teaching;
                if (avgStudent.AttendancePercentage > 0 && avgStudent.AttendancePercentage < margin)
                    persistentAbsentees.Add(avgStudent);
            }
            List<PersistentAbsenteesByYearResponse> persistentAbsenteesByYear = persistentAbsentees.GroupBy(x => x.CourseYear)
                .Select(x => new PersistentAbsenteesByYearResponse
                {
                    Year = int.Parse(x.FirstOrDefault().CourseYear),
                    AttendingStudents = x.Count()
                }).ToList().OrderBy(x => x.Year).ToList();
            IEnumerable<FileRow> persAbsentees = persistentAbsentees.Where(x => x.AttendancePercentage <= (margin - 0.4));
            foreach(FileRow item in persAbsentees)
            {
                PersistentAbsenteesByYearResponse yearRecord = persistentAbsenteesByYear.Find(x => x.Year == int.Parse(item.CourseYear));
                yearRecord.PersistentAbsenteesCount += 1;
                yearRecord.AttendingStudents -= 1;
            }

            return new OkObjectResult(persistentAbsenteesByYear);
        }
        /// <summary>
        /// This method is used to retrieve the number of not attending students by year 
        /// </summary>
        public IActionResult GetNonAttendingCountByYear()
        {
            List<FileRow> notAttendingStudents = new List<FileRow>();
            var students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            //Remodelling data
            foreach (var student in students)
            {
                FileRow avgStudent = new FileRow();
                avgStudent.Id = student.FirstOrDefault().Id;
                avgStudent.UserId = student.FirstOrDefault().UserId;
                avgStudent.StudyLevel = student.FirstOrDefault().StudyLevel;
                avgStudent.CourseYear = student.FirstOrDefault().CourseYear;
                avgStudent.RegStatus = student.FirstOrDefault().RegStatus;
                avgStudent.CourseTitle = student.FirstOrDefault().CourseTitle;
                avgStudent.CourseCode = student.FirstOrDefault().CourseCode;
                foreach (var row in student)
                {
                    avgStudent.Teaching += row.Teaching;
                    avgStudent.Attended += row.Attended;
                    avgStudent.Explained += row.Explained;
                    avgStudent.NonAttended += row.NonAttended;
                }
                avgStudent.AttendancePercentage = (float)avgStudent.Attended / avgStudent.Teaching;
                if (avgStudent.AttendancePercentage == 0)
                    notAttendingStudents.Add(avgStudent);
            }
            List<dynamic> res = new List<dynamic>();
            notAttendingStudents = notAttendingStudents.Where(x => x.AttendancePercentage == 0).ToList();
            var notAttendingStudentsRes = notAttendingStudents.GroupBy(x => x.CourseYear)
                .Select(x => new StudentNonAttendingResponse
                {
                    Year = int.Parse(x.FirstOrDefault().CourseYear),
                    NotAttendingStudents = x.Count()
                }).ToList().OrderBy(x => x.Year).ToList();
            return new OkObjectResult(notAttendingStudentsRes);
        }
        /// <summary>
        /// This method is used to retrieve the persistent absentees count by course with a dynamic risk level margin.
        /// </summary>
        public IActionResult GetPersistentAbsenteesCountByCourse(double margin)
        {
            List<FileRow> persistentAbsentees = new List<FileRow>();
            IEnumerable<List<FileRow>> students = DbContext.Students.AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            //Remodelling data
            foreach (var student in students)
            {
                FileRow avgStudent = new FileRow();
                avgStudent.Id = student.FirstOrDefault().Id;
                avgStudent.UserId = student.FirstOrDefault().UserId;
                avgStudent.StudyLevel = student.FirstOrDefault().StudyLevel;
                avgStudent.CourseYear = student.FirstOrDefault().CourseYear;
                avgStudent.RegStatus = student.FirstOrDefault().RegStatus;
                avgStudent.CourseTitle = student.FirstOrDefault().CourseTitle;
                avgStudent.CourseCode = student.FirstOrDefault().CourseCode;
                foreach (var row in student)
                {
                    avgStudent.Teaching += row.Teaching;
                    avgStudent.Attended += row.Attended;
                    avgStudent.Explained += row.Explained;
                    avgStudent.NonAttended += row.NonAttended;
                }
                avgStudent.AttendancePercentage = (float)avgStudent.Attended / avgStudent.Teaching;
                if (avgStudent.AttendancePercentage > 0 && avgStudent.AttendancePercentage < margin)
                    persistentAbsentees.Add(avgStudent);
            }
            var persistentAbsenteesByCourse = persistentAbsentees.Where(x => x.AttendancePercentage <= (margin - 0.4)).GroupBy(x => x.CourseCode)
                .Select(x => new PersistentAbsenteesByCourse
                {
                    Course = x.FirstOrDefault().CourseCode,
                    Students = x.Count()
                }).ToList().OrderByDescending(x => x.Students).ToList();
            return new OkObjectResult(new { Data = persistentAbsenteesByCourse, Total = persistentAbsentees.Where(x => x.AttendancePercentage <= (margin - 0.4)).Count() });
        }
        /// <summary>
        /// This method is used to retrieve attendance data by periods that a student has attended.
        /// </summary>
        public IActionResult GetAttendanceDataByPeriod()
        {
            List<AbsenceStarting> response = new List<AbsenceStarting>();
            IEnumerable<IEnumerable<FileRow>> GroupedRows = DbContext.Students.ToList().GroupBy(x => x.UserId).Select(x => x.Select(x => x));
            foreach (IEnumerable<FileRow> Row in GroupedRows)
            {
                int index = 0;
                foreach(FileRow x  in Row)
                {
                    if(response.Count() == index)
                    {
                        response.Add(new AbsenceStarting { Attended = x.Attended, Teaching = x.Teaching });
                    }
                    else
                    {
                        response[index].Attended += x.Attended;
                        response[index].Teaching += x.Teaching;
                    }
                    index++;
                }
            }
            return new OkObjectResult(response);
        }
        /// <summary>
        /// This method is used retrieve the average attendance
        /// </summary>
        public IActionResult GetAverageAttendance()
        {
            int attended = 0, sessions = 0;
            foreach(FileRow item in DbContext.Students.ToList())
            {
                attended += item.Attended;
                sessions += item.Teaching;
            }
            return new OkObjectResult(new AverageAttendance {Attended=attended,Sessions=sessions});
        }
        /// <summary>
        /// This method is used to retrieve the students ids that the logged in user is tracking.
        /// </summary>
        public IActionResult GetTrackedStudents(int[] students)
        {
            List<FileRow> dbQuery = DbContext.Students.Where(x => students.Contains(x.UserId)).AsEnumerable().GroupBy(x => x.UserId).Select(x => x.FirstOrDefault()).ToList();
            List<Student> trackedStudents = dbQuery.Select(x => new Student {UserId=x.UserId,StudyLevel=x.StudyLevel,CourseCode=x.CourseCode,CourseTitle=x.CourseTitle }).ToList();
            return new OkObjectResult(trackedStudents);
        }
        /// <summary>
        /// This private method is used remodel data received from the database
        /// </summary>
        private StudentPageResponse GetStudentResponse(List<FileRow> dbQuery)
        {
            List<StudentData> studentData = new List<StudentData>();
            foreach (FileRow record in dbQuery)
            {
                studentData.Add(new StudentData()
                {
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
            StudentPageResponse response = new StudentPageResponse()
            {
                UserId = dbQuery.Select(x => x.UserId).FirstOrDefault(),
                StudyLevel = dbQuery.Select(x => x.StudyLevel).FirstOrDefault(),
                CourseTitle = dbQuery.Select(x => x.CourseTitle).FirstOrDefault(),
                CourseCode = dbQuery.Select(x => x.CourseCode).FirstOrDefault(),
                RegStatus = dbQuery[dbQuery.Count() - 1].RegStatus,
                StudentData = studentData
            };
            return response;
        }
        /// <summary>
        /// This private method is used to prepare students page results that is received from the database.
        /// </summary>
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
