using AttendanceProAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<IActionResult> AddNewEmailData(SendGridEmailRequest email, string folder, string file);
        Task<List<SendGridEmailRequest>> GetEmails(string folder);
    }
}
