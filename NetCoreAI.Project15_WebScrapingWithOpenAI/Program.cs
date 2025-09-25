using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";

    public static async Task Main(string[] args)
    {
        Console.Write("Lütfen analiz yapmak istediğiniz web sayfa URL'ini giriniz: ");
        string inputUrl = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine("Web sayfası içereği: ");
        string webContent = ExtractTextFromWeb(inputUrl);
        await AnalyzeWithAI(webContent, "Web sayfası içeriği");


        static string ExtractTextFromWeb(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var bodyText = doc.DocumentNode.SelectSingleNode("//body")?.InnerText;
            return bodyText ?? "Sayfa içeriği okunamadı.";
        }

        static async Task AnalyzeWithAI(string text, string sourceType)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                    new {role="system",content="Sen bir yapay zeka asistanısın. Kullanıcının gönderdiği metni analiz eder ve Türkçe olarak özetlersin. Yanıtlarını sadece Türkçe ver!"},
                    new{role="user",content=$"Analyze and summarize the following {sourceType}:\n\n {text}"}
                }
                };
                string json = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                    Console.WriteLine($"\n AI Analizi ({sourceType}): \n {result.choices[0].message.content}");
                }
                else
                {
                    Console.WriteLine("Hata: " + responseJson);
                }
            }
        }
    }
}