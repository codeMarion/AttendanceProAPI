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
        private SendGridSettings sendGridSettings;
        private SendGridClient sendGridClient;
        private IBlobStorageService blobService;
        public SendGridService(IOptions<SendGridSettings> sendGridSettings, IBlobStorageService blobService)
        {
            this.blobService = blobService;
            this.sendGridSettings = sendGridSettings.Value;
            sendGridClient = new SendGridClient(this.sendGridSettings.SendGridAPIKey);
        }
        public async Task<IActionResult> ReceiveEmail(InboundEmail email)
        {
            //string[] endOfEmailStrings = { "\n________________________________\n", "\r\n\r\n", "<div class=\"gmail_quote\">" };
            //foreach(string endOfString in endOfEmailStrings)
            //{
            //    if (email.Text.Contains(endOfString))
            //    {
            //        email.Text = email.Text.Split(endOfString)[0];
            //        try
            //        {
            //            email.Html = email.Html.Split(endOfString)[0];
            //        }
            //        catch(Exception ex)
            //        {

            //        }
            //    }
            //}
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
    }
}
