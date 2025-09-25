using System.Text;
using System.Text.Json;
using System.Xml.Linq;

class Program
{
    private static readonly string apiKey = "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";
    private static readonly string rssFeedUrl = "https://www.sozcu.com.tr/rss/tum-haberler.xml";

    static async Task Main()
    {
        Console.WriteLine("Haberler Sistemden Alınıyor...");
        List<string> articles = await FetchLAtestNews(10);

        foreach (var article in articles)
        {
            Console.WriteLine("Haber özeti oluşturuluyor...");
            string summary = await SummarizeArticle(article);
            Console.WriteLine("--- AI tarafından özetlenen haber ---\n");
            Console.WriteLine(summary);
            Console.WriteLine("--------------------------------------------\n");
        }
    }

    static async Task<List<string>> FetchLAtestNews(int count)
    {
        var client = new HttpClient();
        string rssContent = await client.GetStringAsync(rssFeedUrl);

        XDocument doc = XDocument.Parse(rssContent);
        var items = doc.Descendants("item").Take(count);

        List<string> articles = items.Select(item =>
        {
            string title = item.Element("title")?.Value ?? "";
            string description = item.Element("description")?.Value ?? "";
            return $"{title}.{description}";
        }).ToList();
        return articles;
    }

    static async Task<string> SummarizeArticle(string articleText)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var requestBody = new
        {
            model = "gpt-4-turbo",
            messages = new[]
            {
                new {role="system",content="You are an expert news summarizer."},
                new {role="user", content="Bu jaberi 3 cümlede özetle: "+articleText}
            },
            max_tokens = 500
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage respnse = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
        string responseContent = await respnse.Content.ReadAsStringAsync();
        JsonDocument doc = JsonDocument.Parse(responseContent);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }
}
