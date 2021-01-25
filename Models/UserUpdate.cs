using Newtonsoft.Json;

namespace AttendanceProAPI.Models
{
    public class UserUpdate
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
