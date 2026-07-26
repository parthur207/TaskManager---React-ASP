using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class OllamaDTO<T>
    {
        public T? Response { get; set; }    
    }
}
