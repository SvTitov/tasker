using System;
using Newtonsoft.Json;

namespace Tasker.Models.Output
{
    public class LoginOutput
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
