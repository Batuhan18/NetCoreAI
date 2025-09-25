using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "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";

    static async Task Main(string[] args)
    {
        Console.Write("Metni Giriniz: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşturuluyor....");
            await GenerateSpeech(input);
            Console.Write("Ses dosyası 'output mp3' olarak kaydedildi!");
            System.Diagnostics.Process.Start("explorer.exe", "output.mp3");
        }
    }

    static async Task GenerateSpeech(string text)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "tts-1",
                input = text,
                voice = "onyx"
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

            if (response.IsSuccessStatusCode)
            {
                byte[] audioByte = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync("output.mp3", audioByte);
            }
            else
            {
                Console.WriteLine("Bir hata oluştu");
            }
        }
    }
}