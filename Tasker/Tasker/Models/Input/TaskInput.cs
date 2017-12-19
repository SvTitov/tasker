using System;
using Newtonsoft.Json;

namespace Tasker.Models.Input
{
    public class TaskInput
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
