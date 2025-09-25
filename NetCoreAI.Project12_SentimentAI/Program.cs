using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";

    static async Task Main(string[] args)
    {
        Console.Write("Lütfen metni giriniz: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine();
            Console.Write("Duygu Analizi Yaplıyor..");
            Console.WriteLine();
            string sentiment = await AnalyzeSentiment(input);
            Console.WriteLine($"Sonuç: {sentiment}");
        }
    }

    static async Task<string> AnalyzeSentiment(string text)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role= "system", content="You are an AI that analyzes sentiment. You categorize text as Positive, Negative or Neutral." },
                    new {role="user",content=$"Analyze the sentiment of this text: \"{text}\" and return only Positive, Negative or Neutral"}
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
                Console.WriteLine("Bir hata oluştu" + responseJson);
                return "Hata";
            }
        }
    }
}
