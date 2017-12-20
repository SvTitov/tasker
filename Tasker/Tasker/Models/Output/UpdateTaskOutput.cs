using System;
using Newtonsoft.Json;

namespace Tasker.Models.Output
{
    public class UpdateTaskOutput
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
    }
}