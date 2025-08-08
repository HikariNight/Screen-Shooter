using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;

public class Discord{
    public static async Task SendTextAndImageToDiscordAsync(string webhookUrl, string messageText, string imagePath){
        if (!File.Exists(imagePath))
            throw new FileNotFoundException("Image file not found.", imagePath);

        using var client = new HttpClient();
        using var form = new MultipartFormDataContent();

        // Add the message content
        form.Add(new StringContent(messageText), "content");

        // Add the image file
        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
        form.Add(imageContent, "file", Path.GetFileName(imagePath));

        // Send the POST request
        var response = await client.PostAsync(webhookUrl, form);
        response.EnsureSuccessStatusCode(); // throws an exception if the request failed
    }

}