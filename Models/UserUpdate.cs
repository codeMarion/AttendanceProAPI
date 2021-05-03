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

    public class UserUpdateWithMetadata : UserUpdate
    {
        [JsonProperty("user_metadata")]
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("students")]
        public string Students { get; set; }
    }
}
