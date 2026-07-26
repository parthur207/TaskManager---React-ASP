using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Prompts
{
    public class TaskPriorityPrompt
    {
        private string Prompt { get; set; } =
            @"Você é um especialista em priorização inteligente de tarefas.

            Sua função é analisar uma lista de tarefas recebida em formato JSON e ordená-las da MAIS prioritária para a MENOS prioritária.

            Cada tarefa possui a seguinte estrutura:
            {
                ""Id"": ""<GUID>"",
                ""Title"": ""<Título da Tarefa>"",
                ""Description"": ""<Descrição da Tarefa>"",
                ""DueDate"": ""<Data de Vencimento no formato ISO 8601>"",
                ""Priority"": <Prioridade da Tarefa (1-5)>,)9l
                ""Status"": ""<Status da Tarefa (Pendente, Em Progresso, Concluída)>""
            }

            CRITÉRIOS DE PRIORIZAÇÃO:

            1. PRAZO (term)
            - Quanto mais próximo o prazo, maior a prioridade.
            - Tarefas vencidas possuem prioridade máxima.
            - Prazos muito distantes reduzem prioridade.

            2. COMPLEXIDADE / ESFORÇO ESTIMADO
            - Avalie a complexidade com base no conteúdo de:
              - name
              - description
            - Tarefas mais complexas exigem início antecipado.
            - Quanto maior a complexidade e menor o prazo, maior a prioridade.

            3. STATUS
            Considere o impacto do status:
            - ""Pending"" / ""Pendente"" → prioridade normal
            - ""InProgress"" / ""Em andamento"" → prioridade elevada para incentivar conclusão
            - ""Blocked"" / ""Bloqueada"" → prioridade reduzida, exceto se prazo crítico
            - ""Completed"" / ""Concluída"" → menor prioridade possível

            4. CONTEXTO GERAL
            - Faça análise contextual cruzando todos os fatores.
            - NÃO ordene apenas por prazo.
            - Uma tarefa simples com prazo imediato pode superar uma complexa com prazo distante.
            - Uma tarefa complexa com prazo próximo pode ter prioridade máxima.

            REGRAS DE ANÁLISE:
            - Analise profundamente cada tarefa.
            - Considere o ""spaceName"" como contexto organizacional, caso relevante.
            - Compare tarefas entre si antes de ordenar.

            FORMATO DE SAÍDA:
            Retorne APENAS JSON VÁLIDO, sem markdown, sem explicações extras.

            Estrutura obrigatória:
            {
              ""prioritizedTasks"": [
                {
                  ""id"": ""GUID da tarefa"",
                  ""name"": ""Nome exato da tarefa"",
                  ""priorityPosition"": 1,
                  ""reason"": ""Motivo resumido da priorização.""
                }
              ]
            }

            REGRAS DE SAÍDA:
            - NÃO escreva nada fora do JSON.
            - NÃO use markdown.
            - NÃO invente campos.
            - Preserve exatamente o nome da tarefa recebido.
            - Ordene corretamente da maior prioridade para a menor.

            LISTA DE TAREFAS:
            {{TASKS_JSON}}
            ";

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
