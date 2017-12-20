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
            Guid = input.Guid;
        }

        public string Data { get; set; }
        public DateTime Date { get; set; }

        public string Guid { get; set; }
    }
}
