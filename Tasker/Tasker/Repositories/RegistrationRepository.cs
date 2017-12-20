using System;
using System.IO;
using System.Threading.Tasks;
using Flurl.Http;
using Tasker.Models.Output;

namespace Tasker.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        class Some
        {
            public string token { get; set; }
        }

        public async Task<System.Net.Http.HttpResponseMessage> Registration(string phone)
        {
            var result = await (Settings.BaseUrl + "/register")
                            .AllowAnyHttpStatus()
                            .PostJsonAsync(new RegistrationRequestOutput { Phone = phone });
    

            return result;
        }

        public async Task<System.Net.Http.HttpResponseMessage> Confirm (string phone, string code)
        {
            System.Net.Http.HttpResponseMessage result = await (Settings.BaseUrl + "/register/confirm")
                            .AllowAnyHttpStatus()
                            .PostJsonAsync(new ConfirmOutput { Phone = phone , Code = code});

            return result;
        }
    }
}
