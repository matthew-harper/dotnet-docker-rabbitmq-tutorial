using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace worker
{
    public class PostObject
    {
        public string data;
    }
    class Program
    {
    public static async Task PostMessage(string postData)
    {
        var json = JsonConvert.SerializeObject(new PostObject {data = postData});
        var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        using (var httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            using (var client = new HttpClient(httpClientHandler))
            {
                var result = await client.PostAsync("https://localhost:5001/api/Values", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Server returned: " + resultContent);
            }
        }

    }

        static void Main(string[] args)
        {
            Console.WriteLine("Posting a message!");
            PostMessage("test message").Wait();
        }
    }
}
