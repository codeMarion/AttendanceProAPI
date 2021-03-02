using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class PersistentAbsenteesByYearResponse : PersistentAbsenteesResponse
    {
        public int Year { get; set; }
    }
}
