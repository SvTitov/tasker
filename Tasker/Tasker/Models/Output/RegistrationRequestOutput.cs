using System;
using Newtonsoft.Json;

namespace Tasker.Models.Output
{
    public class RegistrationRequestOutput
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
