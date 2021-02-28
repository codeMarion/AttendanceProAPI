using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class PersistentAbsenteesByCourseResponse : PersistentAbsenteesResponse
    {
        public string Course { get; set; }
    }
}
