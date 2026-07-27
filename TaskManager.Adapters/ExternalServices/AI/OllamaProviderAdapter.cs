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

        public async Task<ResponseModel<T>> GenerateAsync<T>(string prompt)
        {
            var responseModel = new ResponseModel<T>();

            if (string.IsNullOrWhiteSpace(prompt))
            {
                responseModel.Status = ResponseStatusEnum.Error;
                responseModel.Message = "O prompt não pode ser nulo ou vazio.";
                return responseModel;
            }

            var request = new
            {
                model = "qwen3:8b",
                prompt,
                stream = false,
                format = "json"
            };

            try
            {
                var httpResponse = await _http.PostAsJsonAsync("api/generate", request);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorBody = await httpResponse.Content.ReadAsStringAsync();

                    responseModel.Status = ResponseStatusEnum.CriticalError;
                    responseModel.Message =
                        $"Ocorreu um erro ao se comunicar com o Ollama ({(int)httpResponse.StatusCode} - {httpResponse.ReasonPhrase}). {errorBody}";

                    return responseModel;
                }

                var ollamaResponse = await httpResponse.Content
                    .ReadFromJsonAsync<OllamaDTO<string>>(_jsonOptions);

                if (ollamaResponse is null || string.IsNullOrWhiteSpace(ollamaResponse.Response))
                {
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message = "O Ollama retornou uma resposta vazia.";
                    return responseModel;
                }

                T? content;

                try
                {
                    content = JsonSerializer.Deserialize<T>(
                        ollamaResponse.Response,
                        _jsonOptions);
                }
                catch (JsonException ex)
                {
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message =
                        $"O JSON retornado pela IA é inválido. {ex.Message}";

                    return responseModel;
                }

                if (content is null)
                {
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message =
                        "Não foi possível converter o JSON retornado pela IA para o tipo esperado.";

                    return responseModel;
                }

                responseModel.Status = ResponseStatusEnum.Success;
                responseModel.Content = content;

                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message =
                    $"Falha de comunicação com o Ollama. Verifique se o serviço está acessível na rede. Detalhes: {ex.Message}";

                return responseModel;
            }
            catch (TaskCanceledException ex)
            {
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message =
                    $"Tempo limite excedido ao aguardar resposta do Ollama. Detalhes: {ex.Message}";

                return responseModel;
            }
            catch (JsonException ex)
            {
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message =
                    $"Falha ao interpretar a resposta do Ollama. Detalhes: {ex.Message}";

                return responseModel;
            }
        }
    }
}