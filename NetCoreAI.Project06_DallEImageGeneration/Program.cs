
using Newtonsoft.Json;
using System.Text;

class Program
{
    //Girilen prompta göre görsel verir
    public static async Task Main(string[] args)
    {
        string apiKey = "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";
        Console.Write("Example prompts: ");
        string prompt = Console.ReadLine();
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                prompt = prompt,
                n = 1,
                size = "1024x1024"
            };
            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }
}

/*
 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
 */