using AttendanceProAPI.Data;
using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services
{
    public class SendGridService : ISendGridService
    {
        private DataContext DbContext;
        private SendGridSettings sendGridSettings;
        private SendGridClient sendGridClient;
        private IBlobStorageService blobService;
        public SendGridService(IOptions<SendGridSettings> sendGridSettings, IBlobStorageService blobService, DataContext DbContext)
        {
            this.DbContext = DbContext;
            this.blobService = blobService;
            this.sendGridSettings = sendGridSettings.Value;
            sendGridClient = new SendGridClient(this.sendGridSettings.SendGridAPIKey);
        }
        public async Task<IActionResult> ReceiveEmail(InboundEmail email)
        {
            List<SendGridEmailRequest> emails = await blobService.GetEmails(email.From.Split("<")[1].Split(">")[0]);
            if (email.Html != null)
            {
                email.Html = email.Html.Replace("\n", "<br>");
            }
            

            return await blobService.AddNewEmailData(
                new SendGridEmailRequest()
                {
                    Content = email.Text,
                    FromEmail = email.From.Split("<")[1].Split(">")[0],
                    FromName = email.From.Split("<")[0],
                    Date = DateTime.Now,
                    ToEmail = email.To,
                    ToName = email.To,
                    Subject = email.Subject,
                    HtmlContent = email.Html
                },
                email.From.Split("<")[1].Split(">")[0],
                (emails.Count() + 1).ToString()
            );
        }

        public async Task<IActionResult> SendEmail(SendGridEmailRequest email, string folder)
        {
            List<SendGridEmailRequest> emails = await blobService.GetEmails(email.ToEmail);
            email.Date = DateTime.Now;
            email.Content = email.Content.Replace("\n", "<br>");
            email.HtmlContent = email.HtmlContent.Replace("\n", "<br>");
            await blobService.AddNewEmailData(email, folder, (emails.Count() + 1).ToString());
            SendGridMessage message = MailHelper.CreateSingleEmail(
                new EmailAddress(email.FromEmail, email.FromName),
                new EmailAddress(email.ToEmail, email.ToName),
                email.Subject,
                email.Content,
                email.HtmlContent
            );
            await sendGridClient.SendEmailAsync(message);
            return new OkResult();
        }

        public async Task<IActionResult> GetEmails(string id)
        {
            return new OkObjectResult(await blobService.GetEmails(id));
        }

        public async Task<IActionResult> SendRemindersMessages()
        {
            List<FileRow> studentsToBeEmailed = new List<FileRow>();
            List<int> ids = DbContext.PersonalDetails.Select(x => x.UserId).ToList();
            IEnumerable<List<FileRow>> studentRecords = DbContext.Students.Where(x => ids.Contains(x.UserId)).AsEnumerable().GroupBy(x => x.UserId).Select(x => x.ToList());
            foreach (List<FileRow> student in studentRecords)
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
                if(avgStudent.AttendancePercentage < 0.8)
                {
                    PersonalDetails details = DbContext.PersonalDetails.Where(x => x.UserId == avgStudent.UserId).FirstOrDefault();
                    await SendEmail(new SendGridEmailRequest
                    {
                        Content=$"Dear Student #{details.UserId}<br />Please attend your classes<br />Your attendance is current below 80%<br />Kindest Regards<br />School Office",
                        HtmlContent= "Please attend your classes<br />School Office",
                        Date=DateTime.Now,
                        FromEmail= "admin@em2322.attendancepro.co.uk",
                        FromName="AttendancePro",
                        Subject="Attendance Reminder",
                        ToEmail=details.Email,
                        ToName=$"Student #{details.UserId}"
                    },details.Email);
                }
            }
            return new OkResult();
        }
    }
}
