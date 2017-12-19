using System;
using Newtonsoft.Json;

namespace Tasker.Models.Output
{
    public class ConfirmOutput
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
