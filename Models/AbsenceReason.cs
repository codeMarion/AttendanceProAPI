using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class AbsenceReason
    {
        public int Teaching { get; set; }
        public int Attended { get; set; }
        public int Explained { get; set; }
        public int NonAttended { get; set; }
    }

    public class AbsenceReasonByYear : AbsenceReason
    {
        public int Year { get; set; }
    }
}
