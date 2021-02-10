using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class SendGridSettings
    {
        public string SendGridAPIKey { get; set; }
        public string BlobConnectionString { get; set; }
        public string BlobContainer { get; set; }
    }
}
