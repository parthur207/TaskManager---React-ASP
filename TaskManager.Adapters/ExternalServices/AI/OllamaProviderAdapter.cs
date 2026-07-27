using System.Net.Http.Json;
using System.Text.Json;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.ExternalServices.AI
{
    public class OllamaProviderAdapter : IOllamaProviderPort
    {
        private readonly HttpClient _http;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public OllamaProviderAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<ResponseModel<object>> GenerateAsync(string prompt)
        {
            var Response = new ResponseModel<object>();

            if (string.IsNullOrWhiteSpace(prompt))
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "O prompt não pode ser nulo ou vazio.";
                return Response;
            }

            var request = new
            {
                model = "qwen3:8b",
                prompt,
                stream = false
            };

            try
            {
                var response = await _http.PostAsJsonAsync("api/generate", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Response.Status = ResponseStatusEnum.CriticalError;
                    Response.Message = $"Ocorreu um erro ao se comunicar com o Ollama ({(int)response.StatusCode} - {response.ReasonPhrase}). {errorBody}";
                    return Response;
                }

                var result = await response.Content
                    .ReadFromJsonAsync<OllamaDTO<string>>(_jsonOptions);

                if (result is null || string.IsNullOrWhiteSpace(result.Response))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Ocorreu um erro ao processar a resposta do Ollama.";
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = result.Response;
                return Response;
            }
            catch (HttpRequestException ex)
            {
                Response.Status = ResponseStatusEnum.CriticalError;
                Response.Message = $"Falha de comunicação com o Ollama. Verifique se o serviço está acessível na rede. Detalhes: {ex.Message}";
                return Response;
            }
            catch (JsonException ex)
            {
                Response.Status = ResponseStatusEnum.CriticalError;
                Response.Message = $"Falha ao interpretar a resposta do Ollama. Detalhes: {ex.Message}";
                return Response;
            }
            catch (TaskCanceledException ex)
            {
                Response.Status = ResponseStatusEnum.CriticalError;
                Response.Message = $"Tempo limite excedido ao aguardar resposta do Ollama. Detalhes: {ex.Message}";
                return Response;
            }
        }
    }
}