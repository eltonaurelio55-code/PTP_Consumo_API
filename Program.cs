using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consumo_API_CSharp
{
    /// <summary>
    /// Modelos de dados que correspondem à estrutura JSON devolvida pela API DummyJSON Quotes.
    /// </summary>
    public class Quote
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("quote")]
        public string Texto { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Autor { get; set; } = string.Empty;
    }

    public class QuoteListResponse
    {
        [JsonPropertyName("quotes")]
        public List<Quote> Quotes { get; set; } = new();

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("skip")]
        public int Skip { get; set; }

        [JsonPropertyName("limit")]
        public int Limite { get; set; }
    }

    internal static class Program
    {
        // Endpoint da API escolhida: DummyJSON Quotes (citações)
        private const string ApiUrl = "https://dummyjson.com/quotes?limit=10";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ExibirCabecalho();

            try
            {
                using HttpClient client = CriarHttpClient();

                Console.WriteLine($"A contactar a API: {ApiUrl}\n");
                HttpResponseMessage resposta = await client.GetAsync(ApiUrl);

                // Garante que a API devolveu um código de sucesso (2xx)
                resposta.EnsureSuccessStatusCode();

                string json = await resposta.Content.ReadAsStringAsync();
                QuoteListResponse? dados = JsonSerializer.Deserialize<QuoteListResponse>(json, JsonOptions);

                if (dados == null || dados.Quotes == null || dados.Quotes.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[!] A API não devolveu nenhuma citação.");
                    Console.ResetColor();
                    return 1;
                }

                ExibirResultados(dados);
                return 0;
            }
            catch (HttpRequestException ex)
            {
                // Falha de conexão, DNS, timeout HTTP ou código de erro HTTP
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERRO DE REDE] Não foi possível contactar a API.");
                Console.WriteLine($"Detalhe: {ex.Message}");

                if (ex.StatusCode is not null)
                {
                    Console.WriteLine($"Código HTTP: {ex.StatusCode}");
                }
                Console.ResetColor();
                return 2;
            }
            catch (TaskCanceledException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERRO DE REDE] A ligação expirou (timeout). Verifique a sua conexão à Internet.");
                Console.ResetColor();
                return 3;
            }
            catch (JsonException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERRO DE JSON] A resposta recebida não pode ser convertida em objetos C#.");
                Console.WriteLine($"Detalhe: {ex.Message}");
                Console.ResetColor();
                return 4;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERRO INESPERADO] {ex.GetType().Name}: {ex.Message}");
                Console.ResetColor();
                return 5;
            }
            finally
            {
                Console.ResetColor();
                Console.WriteLine("\nPrograma terminado.");

                // Só aguarda tecla quando existe console interactivo
                if (!Console.IsInputRedirected)
                {
                    Console.WriteLine("Prima qualquer tecla para sair...");
                    Console.ReadKey(true);
                }
            }
        }

        private static HttpClient CriarHttpClient()
        {
            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(15) // Evita esperas infinitas por rede lenta
            };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Consumo_API_CSharp/1.0");
            return client;
        }

        private static void ExibirCabecalho()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================================");
            Console.WriteLine("   TRABALHO PRÁTICO - CONSUMO DE WEB API EM C#");
            Console.WriteLine("   Projeto: Consumo_API_CSharp");
            Console.WriteLine("   API: DummyJSON Quotes (https://dummyjson.com/quotes)");
            Console.WriteLine("==========================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ExibirResultados(QuoteListResponse dados)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Resposta recebida com sucesso! Total de citações na API: {dados.Total}");
            Console.ResetColor();
            Console.WriteLine($"A apresentar {dados.Quotes.Count} citações (skip={dados.Skip}, limit={dados.Limite}):\n");

            foreach (Quote q in dados.Quotes)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"#{q.Id} ");
                Console.ResetColor();
                Console.WriteLine($"\"{q.Texto}\"");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     — {q.Autor}\n");
                Console.ResetColor();
            }
        }
    }
}
