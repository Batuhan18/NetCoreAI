using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";

    static async Task Main(string[] args)
    {
        Console.Write("Uzun metninizi veya makalenizi girin: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine();
            Console.WriteLine("Girmiş olduğunuz metin AI tarafından özetleniyor....");
            Console.WriteLine();

            string shortSummary = await SummarizeText(input, "short");
            string mediumSummary = await SummarizeText(input, "medium");
            string detailSummary = await SummarizeText(input, "detail");

            Console.WriteLine("Özetler");
            Console.WriteLine("-------------------");
            Console.WriteLine($" ** Kısa Özet: ** {shortSummary}");
            Console.WriteLine("-------------------");
            Console.WriteLine($" ** Orta Uzunlukta Özet: ** {mediumSummary}");
            Console.WriteLine("-------------------");
            Console.WriteLine($" ** Detaylı Özet: ** {detailSummary}");
        }

    }


    public static async Task<string> SummarizeText(string text, string level)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            string instruction = level switch
            {
                "short" => "Summarize this text in 1-2 sentences.",
                "medium" => "Summarize tihs text in 3-4 sentences.",
                "detail" => "Summarize this text in a detailed but concise manner.",
                _ => "Summarize this text."
            };

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are an AI that summarize text info different levels: short, medium and detailed." },
                    new { role = "user", content = $"{instruction}\n\n{text}" }
                }
            };
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            string responseJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result.choices[0].message.content.ToString();
            }
            else
            {
                Console.WriteLine($"Hata: {responseJson}");
                return "Hata!";
            }
        }
    }
}
