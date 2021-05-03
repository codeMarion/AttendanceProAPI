using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class Auth0Settings
    {
        public string Auth0Domain { get; set; }
        public string Auth0Audience { get; set; }
        public string Auth0ManagementAPIKey { get; set; }
    }
}
