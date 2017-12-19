using System;
using System.Threading.Tasks;
using Tasker.Models.Input;
using Tasker.Models.Output;

namespace Tasker.Repositories
{
    public interface ILoginRepository
    {
        Task<LoginInput> Login(LoginOutput model);
    }
}
