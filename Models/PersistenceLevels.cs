using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Models
{
    public class PersistenceLevels
    {
        [JsonProperty("critical")]
        public int Critical { get; set; }
        [JsonProperty("veryBad")]
        public int VeryBad { get; set; }
        [JsonProperty("bad")]
        public int Bad { get; set; }
    }
}
