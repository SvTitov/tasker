using System;
using System.Collections.Generic;
using Flurl.Http;
using Tasker.Models.System;
using Tasker.Models.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Tasker.Repositories
{
    public class RestRepository : IRestRepository
    {
        public async Task<List<Models.System.Task>> GetTasks()
        {
            var response = await Settings.BaseUrl.AllowHttpStatus()
                         .GetJsonAsync<List<TaskInput>>();

            return response.Select(item => new Models.System.Task(item)).ToList();
        }
    }
}
