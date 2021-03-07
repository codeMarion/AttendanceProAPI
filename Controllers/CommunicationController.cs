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

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendGridEmailRequest email)
        {
            await sendGridService.SendEmail(email, email.ToEmail);
            return new OkResult();
        }

        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveEmail([FromForm] InboundEmail emailData)
        {
            await sendGridService.ReceiveEmail(emailData);
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmails(string id)
        {
            return await sendGridService.GetEmails(id);
        }

        [HttpPost("reminders")]
        public async Task<IActionResult> SendRemindersMessages()
        {
            return await sendGridService.SendRemindersMessages();
        }
    }
}
