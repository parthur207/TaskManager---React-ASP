using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Prompts
{
    public class RefineTaskAttributesPrompt
    {
        private string Prompt { get; set; } =
        @"";

        public string PromptBuilder(TaskDTO Task)
        {
            Prompt = Prompt.Replace("{{TASK_JSON}}", System.Text.Json.JsonSerializer.Serialize(new TaskDTO
            {
                Title = Task.Title,
                Description = Task.Description,
                Status = Task.Status,
                Term = Task.Term
            }));

            return Prompt;
        }
    }
}
