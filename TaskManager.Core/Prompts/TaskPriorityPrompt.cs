using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Prompts
{
    public class TaskPriorityPrompt
    {
        public string Prompt { get; private set; } =
            "Você é um especialista em priorização inteligente de tarefas." +
            "Sua função é analisar uma lista de tarefas recebida em formato JSON" +
            " e ordená-las da MAIS prioritária para a MENOS prioritária." +
            "Cada tarefa possui a seguinte estrutura:" +
            "{\r\n  \"name\": " +
            "\"string\",\r\n  " +
            "\"description\": \"string | null\",\r\n  " +
            "\"spaceName\": \"string\",\r\n  " +
            "\"status\": \"string\",\r\n  " +
            "\"term\": \"yyyy-MM-dd\"\r\n}\r\n\r\n" +
            "CRITÉRIOS DE PRIORIZAÇÃO:\r\n\r\n" +
            "1. PRAZO (term) - Quanto mais próximo o prazo, maior a prioridade.\r\n" +
            "- Tarefas vencidas possuem prioridade máxima.\r\n" +
            "- Prazos muito distantes reduzem prioridade.\r\n\r\n" +
            "2. COMPLEXIDADE / ESFORÇO ESTIMADO\r\n- Avalie a complexidade com base no conteúdo de:\r\n  - name\r\n  - description\r\n- Tarefas mais complexas exigem início antecipado.\r\n- Quanto maior a complexidade e menor o prazo, maior a prioridade.\r\n\r\n3. STATUS\r\nConsidere o impacto do status:\r\n- \"Pending\" / \"Pendente\" → prioridade normal\r\n- \"InProgress\" / \"Em andamento\" → prioridade elevada para incentivar conclusão\r\n- \"Blocked\" / \"Bloqueada\" → prioridade reduzida, exceto se prazo crítico\r\n- \"Completed\" / \"Concluída\" → menor prioridade possível\r\n\r\n4. CONTEXTO GERAL\r\n- Faça análise contextual cruzando todos os fatores.\r\n- NÃO ordene apenas por prazo.\r\n- Uma tarefa simples com prazo imediato pode superar uma complexa com prazo distante.\r\n- Uma tarefa complexa com prazo próximo pode ter prioridade máxima.\r\n\r\nREGRAS DE ANÁLISE:\r\n- Analise profundamente cada tarefa.\r\n- Considere o \"spaceName\" como contexto organizacional, caso relevante.\r\n- Compare tarefas entre si antes de ordenar.\r\n\r\nFORMATO DE SAÍDA:\r\nRetorne APENAS JSON VÁLIDO, sem markdown, sem explicações extras.\r\n\r\nEstrutura obrigatória:\r\n\r\n{\r\n  \"prioritizedTasks\": [\r\n    {\r\n      \"name\": \"Nome exato da tarefa\",\r\n      \"priorityPosition\": 1,\r\n      \"reason\": \"Motivo resumido da priorização.\"\r\n    }\r\n  ]\r\n}\r\n\r\nREGRAS DE SAÍDA:\r\n- NÃO escreva nada fora do JSON.\r\n- NÃO use markdown.\r\n- NÃO invente campos.\r\n- Preserve exatamente o nome da tarefa recebido.\r\n- Ordene corretamente da maior prioridade para a menor.\r\n\r\nLISTA DE TAREFAS:\r\n{{}}";
    
        public void PromptBuilder(IList<TaskEntity>)
        {

        }        
    }
}
