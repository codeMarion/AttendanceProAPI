using AttendanceProAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface ISendGridService
    {
        Task<IActionResult> SendEmail(SendGridEmailRequest email, string folder);
        Task<IActionResult> ReceiveEmail(InboundEmail email);
        Task<IActionResult> GetEmails(string id);
        Task<IActionResult> SendRemindersMessages();
    }
}
