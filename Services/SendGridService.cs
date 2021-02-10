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
            return await blobService.AddNewEmailData(
                        new SendGridEmailRequest()
                        {
                            Content = email.Text,
                            FromEmail = email.From.Split("<")[1].Split(">")[0],
                            FromName = email.From.Split("<")[0],
                            ToEmail = email.To,
                            ToName = email.To,
                            Subject = email.Subject,
                            HtmlContent = email.Html
                        },
                        email.From.Split("<")[1].Split(">")[0],
                        Guid.NewGuid().ToString()
                    );
        }

        public async Task<IActionResult> SendEmail(SendGridEmailRequest email, string folder, string file)
        {
            await blobService.AddNewEmailData(email, folder, file);
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
            return await blobService.GetEmails(id);
        }
    }
}
