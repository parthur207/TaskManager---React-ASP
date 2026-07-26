using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.ExternalServices.AI
{
    public class OllamaProviderAdapter : IOllamaProviderPort
    {
        private readonly HttpClient _http;

        public OllamaProviderAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<ResponseModel<object>> GenerateAsync(string prompt)
        {
            var Response = new ResponseModel<object>();

            var request = new
            {
                model = "qwen3:8b",
                prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync(
                "api/generate",
                request);

            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                Response.Message = $"Ocorreu um erro inesperado ao se comunicar com o Ollama: {response.ReasonPhrase}";
                Response.Status = ResponseStatusEnum.CriticalError;
                return Response;
            }

            var result = await response.Content.ReadFromJsonAsync<OllamaDTO<object>>();

            if (result is null)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "Ocorreu um erro ao processar a resposta do Ollama.";
                return Response;
            }

            Response.Status = ResponseStatusEnum.Success;
            Response.Content = result;

            return Response;
        }
    }
}
