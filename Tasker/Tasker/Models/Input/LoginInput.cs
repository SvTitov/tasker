using System;
using Newtonsoft.Json;

namespace Tasker.Models.Input
{
    public class LoginInput
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
