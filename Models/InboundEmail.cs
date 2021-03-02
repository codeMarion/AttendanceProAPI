using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class InboundEmail
    {
        public string Dkim { get; set; }
        public string To { get; set; }
        public string Html { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public string SenderIp { get; set; }
        public string Envelope { get; set; }
        public int Attachments { get; set; }
        public string Subject { get; set; }
        public string Charsets { get; set; }
        public string Spf { get; set; }
    }
}
