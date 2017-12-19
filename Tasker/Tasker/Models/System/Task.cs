using System;
using Tasker.Models.Input;

namespace Tasker.Models.System
{
    public class Task
    {
        public Task(TaskInput input)
        {
            Data = input.Data;
            Date = input.Date;
        }

        public string Data { get; set; }
        public DateTime Date { get; set; }
    }
}
