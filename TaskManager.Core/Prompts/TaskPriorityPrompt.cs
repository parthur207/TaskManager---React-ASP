using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Prompts
{
    public class TaskPriorityPrompt
    {
        public string Prompt { get; private set; } =
            @"Você é um especialista em priorização inteligente de tarefas.

            Sua função é analisar uma lista de tarefas recebida em formato JSON e ordená-las da MAIS prioritária para a MENOS prioritária.

            Cada tarefa possui a seguinte estrutura:
            {
              ""title"": ""string"",
              ""description"": ""string | null"",
              ""spaceName"": ""string"",
              ""status"": ""string"",
              ""term"": ""yyyy-MM-dd""
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
        public JsonContent PromptBuilder(IList<TaskEntity> Dados)
        {
            return JsonContent.Create(new
            {
                prompt = Prompt.Replace("{{TASKS_JSON}}", System.Text.Json.JsonSerializer.Serialize(Dados.Select(t => new
                {
                    title = t.Title,
                    description = t.Description,
                    spaceName = t.Space.Name,
                    status = t.StatusEnum.ToString(),
                    term = t.Term.ToString("yyyy-MM-dd")
                })))
            });
        }        
    }
}
