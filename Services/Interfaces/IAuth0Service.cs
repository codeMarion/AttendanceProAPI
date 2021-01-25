using AttendanceProAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services.Interfaces
{
    public interface IAuth0Service
    {
        Task<IActionResult> UpdateUserDetails(string id, UserUpdate user);
    }
}
