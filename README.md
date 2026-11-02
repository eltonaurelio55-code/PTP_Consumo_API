# PTP_Consumo_API

Trabalho Prático: **Consumo de Web API em C#** — Projeto `Consumo_API_CSharp`.

## Descrição

Aplicação em **C# (.NET Console)** que consome a Web API pública
[DummyJSON Quotes](https://dummyjson.com/quotes) usando a classe `HttpClient`,
converte a resposta JSON em objetos estruturados com `System.Text.Json` e
apresenta os dados na consola, incluindo tratamento de exceções para falhas de rede.

## API utilizada

- **Endpoint:** `https://dummyjson.com/quotes?limit=10`
- **Método HTTP:** `GET`
- **Formato da resposta:** JSON com as propriedades `quotes`, `total`, `skip` e `limit`

Exemplo de registo devolvido pela API:

```json
{
  "id": 1,
  "quote": "Your heart is the size of an ocean. Go find yourself in its hidden depths.",
  "author": "Rumi"
}
```

## Como executar

Pré-requisito: [.NET SDK](https://dotnet.microsoft.com/download) (8.0 ou superior).

```powershell
dotnet run --project Consumo_API_CSharp
```

Ou compilar primeiro:

```powershell
dotnet build Consumo_API_CSharp/Consumo_API_CSharp.csproj
dotnet run --project Consumo_API_CSharp
```

## Estrutura do projeto

```
Consumo_API_CSharp/
├── Consumo_API_CSharp.csproj   # Ficheiro de projeto .NET
├── Program.cs                  # Lógica: HttpClient, modelos JSON, tratamento de erros e apresentação
├── README.md                   # Este ficheiro
└── .gitignore                  # Ignora bin/, obj/, etc.
```

## Funcionalidades

1. **Requisição HTTP** — `HttpClient` com `GET` para o endpoint da API.
2. **Conversão JSON** — `System.Text.Json` com atributos `[JsonPropertyName]`
   (`Quote` e `QuoteListResponse`).
3. **Tratamento de erros** — blocos `try-catch` para:
   - `HttpRequestException` (falhas de conexão / erros HTTP)
   - `TaskCanceledException` (timeout da ligação)
   - `JsonException` (JSON inválido)
   - `Exception` (erros inesperados)
4. **Apresentação** — cabeçalho, confirmação de sucesso e lista formatada
   das citações com autor.

## Autor

- **Nome:** Elton Banze
- **Disciplina:** Trabalho Prático — Consumo de Web API em C#
- **Prazo:** 03 de novembro de 2026
