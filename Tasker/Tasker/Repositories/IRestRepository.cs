using System;
using System.Collections.Generic;
using Flurl.Http;
using Tasker.Models.System;
using Tasker.Models.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Tasker.Repositories
{
    public interface IRestRepository
    {
        Task<List<Models.System.Task>> GetTasks();

        Task<bool> UpdateTask(string guid, string data, DateTime date);

        Task<bool> DeleteTask(string guid);

        Task<bool> AddTask(string data, DateTime date, string guid);

        Task<TaskInput> GetTask(string guid);
    }
}