using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class AI_PriorityTasksDTO
    {
        public Guid TaskId { get; set; }
        public int PrioritySort { get; set; }
        public string Justify { get; set; }
    }
}
