
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = "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";
        Console.WriteLine("Sorunuzu yazınız:(örnek:'Merhaba bugün Ankara'da hava kaç derece')");

        var prompt = Console.ReadLine();
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new {role="system",content="You are a helpful assistant."},
                new {role="user",content=prompt}
            },
            max_tokens = 1000
        };
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                Console.WriteLine("Open AI'nin Cevabı: ");
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine($"Bir hata oluştu: {response.StatusCode}");
                Console.WriteLine(responseString);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Bir hata oluştu: {ex.Message}");
        }
    }
}

/*
 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
 */