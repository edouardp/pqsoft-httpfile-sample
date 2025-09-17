using PQSoft.HttpFile;
using System.Net.Http;
using System.Text;

var httpFile = "sample.http";
var httpClient = new HttpClient();

try
{
    using var stream = new FileStream(httpFile, FileMode.Open, FileAccess.Read);
    
    await foreach (var request in HttpFileParser.ParseAsync(stream))
    {
        Console.WriteLine($"Executing: {request.Method} {request.Url}");
        
        var httpRequest = request.ToHttpRequestMessage();
        
        var response = await httpClient.SendAsync(httpRequest);
        var content = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Response: {content[..Math.Min(200, content.Length)]}...\n");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    httpClient.Dispose();
}
