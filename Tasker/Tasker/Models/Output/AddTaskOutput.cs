using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tasker.Models.Output
{
    public class AddTaskOutput
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
    }
}