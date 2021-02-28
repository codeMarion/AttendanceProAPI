﻿using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Controllers
{
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
            await sendGridService.SendEmail(email, email.ToEmail, Guid.NewGuid().ToString());
            return new OkResult();
        }

        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveEmail([FromForm] InboundEmail emailData)
        {
            return await sendGridService.ReceiveEmail(emailData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmails(string id)
        {
            return await sendGridService.GetEmails(id);
        }
    }
}