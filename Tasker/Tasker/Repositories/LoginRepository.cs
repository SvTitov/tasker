using System;
using System.IO;
using System.Threading.Tasks;
using Tasker.Models.Output;
using Flurl.Http;
using Tasker.Models.Input;

namespace Tasker.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        public async Task<LoginInput> Login(LoginOutput model)
        {
            return await (Settings.BaseUrl + "/login")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model)
                .ReceiveJson<LoginInput>();
        }
    }
}
