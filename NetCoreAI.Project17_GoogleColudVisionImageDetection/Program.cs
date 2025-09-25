using System.Text;
using System.Text.Json;

class Program
{
    private static readonly string googleApiKey = "";
    private static readonly string imagePath = "";

    static async Task Main()
    {
        Console.WriteLine("Google Vision Api ile Görsel Nesne Tespiti Yapılıyor....");
        string response = await DetectObject(imagePath);
        Console.WriteLine("-----Tespit Edilen Nesneler-----");
        Console.WriteLine(response);
    }

    static async Task<string> DetectObject(string path)
    {
        using var client = new HttpClient();

        string apiUrl = $"https://vision.googleapis.com/v1/images:annotate?kry={googleApiKey}";

        byte[] imageBytes = File.ReadAllBytes(path);
        string base64Image = Convert.ToBase64String(imageBytes);

        var requestBody = new
        {
            requests = new[]
            {
                new
                {
                    image = new { content = base64Image },
                    features = new[] { new { type = "LABEL_DETECTION", maxResults = 10 } }

                } }
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}


