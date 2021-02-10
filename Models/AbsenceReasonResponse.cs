using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class AbsenceReasonResponse
    {
        public AbsenceReason Overall { get; set; }
        public List<AbsenceReasonByYear> AbsenceReasons { get; set; }
    }
}
