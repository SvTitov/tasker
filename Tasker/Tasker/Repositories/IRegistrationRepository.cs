using System;
using System.Threading.Tasks;

namespace Tasker.Repositories
{
    public interface IRegistrationRepository
    {
        Task<System.Net.Http.HttpResponseMessage> Confirm(string phone, string code);

        Task<System.Net.Http.HttpResponseMessage> Registration(string phone);
    }
}
