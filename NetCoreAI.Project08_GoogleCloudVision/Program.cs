using Google.Cloud.Vision.V1;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Resim yolunu giriniz:");
        Console.WriteLine();
        string imagePath = Console.ReadLine();

        string credentialPath = @"C:\Users\batuh\OneDrive\Desktop\able-air-471908-m0-9beec2dcc67b.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        try
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile(imagePath);
            var response = client.DetectText(image);
            Console.WriteLine("Resimdeki metin:");
            Console.WriteLine();
            foreach (var annotation in response)
            {
                if (!string.IsNullOrEmpty(annotation.Description))
                {
                    Console.WriteLine(annotation.Description);
                }
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Bir hata oluştu {ex.Message}");
        }
    }
}
