using System;
using System.Collections.Generic;
using Flurl.Http;
using Tasker.Models.System;
using Tasker.Models.Input;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Tasker.Extensions;
using Tasker.Models.Output;
using Task = Tasker.Models.System.Task;

namespace Tasker.Repositories
{
    public class RestRepository : IRestRepository
    {
        public async Task<List<Models.System.Task>> GetTasks()
        {
            var response = await (Settings.BaseUrl + "/tasks")
                         .AllowAnyHttpStatus()
                         .AddAuthorizationHeader()
                         .GetJsonAsync<List<TaskInput>>();

            return response == null ? new List<Task>()
                                    : response?.Select(item => new Models.System.Task(item)).ToList();
        }

        public async Task<bool> UpdateTask(string guid, string data, DateTime date)
        {
            var httpResponseMessage = await (Settings.BaseUrl + "/task")
                .AllowAnyHttpStatus()
                .AddAuthorizationHeader()
                .PutJsonAsync(new UpdateTaskOutput {Data = data, Date = date, Guid = guid});

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTask(string guid)
        {
            var result = await (Settings.BaseUrl + "/task")
                .AllowAnyHttpStatus()
                .AddAuthorizationHeader()
                .SendJsonAsync(HttpMethod.Delete, new {guid = guid});

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddTask(string data, DateTime date, string guid)
        {
            var message = await (Settings.BaseUrl + "/task")
                .AllowAnyHttpStatus()
                .AddAuthorizationHeader()
                .PostJsonAsync(new AddTaskOutput {Date = date, Guid = guid, Data = data});

            return message.IsSuccessStatusCode;
        }

        public async Task<TaskInput> GetTask(string guid)
        {
            var result = await Settings.BaseUrl.AppendPathSegment("task")
                .SetQueryParam("id", guid)
                .AllowAnyHttpStatus()
                .AddAuthorizationHeader()
                .GetJsonAsync<TaskInput>();

            return result;
        }
    }
}
