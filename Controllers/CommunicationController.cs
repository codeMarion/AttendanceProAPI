using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AttendanceProAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private ISendGridService sendGridService;
        public CommunicationController(ISendGridService sendGridService)
        {
            this.sendGridService = sendGridService;
        }
        /// <summary>
        /// This endpoint is used to send an email.
        /// </summary>
        /// <param name="email">The email content with relevant headers</param>
        /// <response code="200">Email sent successfully</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendGridEmailRequest email)
        {
            await sendGridService.SendEmail(email, email.ToEmail);
            return new OkResult();
        }
        /// <summary>
        /// This endpoint is used as a webhook for inbound emails from Twilio SendGrid
        /// </summary>
        /// <param name="emailData">The email content with relevant SendGrid headers</param>
        /// <response code="200">Email receieved successfully</response>
        [AllowAnonymous]
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveEmail([FromForm] InboundEmail emailData)
        {
            await sendGridService.ReceiveEmail(emailData);
            return new OkResult();
        }
        /// <summary>
        /// Gets all emails that are linked to a given student
        /// </summary>
        /// <param name="id">email address of a student</param>
        /// <returns>Retrieves all emails that are linked to a given student</returns>
        /// <response code="200">Returns all emails linked to a gives student</response>
        /// <response code="400">Unauthorised result</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmails(string id)
        {
            return await sendGridService.GetEmails(id);
        }
        /// <summary>
        /// HttpTrigger to send automatic reminder emails to risk students.
        /// </summary>
        /// <response code="200">Sends email reminders to risk students</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPost("reminders")]
        public async Task<IActionResult> SendRemindersMessages()
        {
            return await sendGridService.SendRemindersMessages();
        }
    }
}
