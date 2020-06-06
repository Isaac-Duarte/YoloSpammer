using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YoloSpammer
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.Write("YOLO Id (ex. VHRt8t9P7b): ");
            string yoloId = Console.ReadLine();

            Console.Write("Enter the message you want to spam: ");
            string message = Console.ReadLine();

            AskForIterations:
            
            Console.Write("Number of times you want to send the message: ");
            string iterationsInput = Console.ReadLine();
            int iterations;

            if (!int.TryParse(iterationsInput, out iterations))
            {
                goto AskForIterations;
            }

            await SendYolo(yoloId, message, iterations);

            Console.WriteLine("Done!");
            Console.WriteLine("Press any button to close.");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Generates a fake cookie
        /// </summary>
        /// <returns>Generated cookie</returns>
        public static string NewCookie()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            
            for (int i = 0; i < 22; i++)
            {
                stringBuilder.Append(valid[random.Next(valid.Length)]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Sends a yolo
        /// </summary>
        /// <param name="yoloId">ID of the Yolo</param>
        /// <param name="message">Message of the yolo</param>
        /// <param name="iterations">How many times to send the message</param>
        /// <returns></returns>
        public static async Task SendYolo(string yoloId, string message, int iterations)
        {
            string cookie = NewCookie();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("cookie", $"popshow-temp-id={cookie}");
            client.DefaultRequestHeaders.Add("origin", "https://onyolo.com");
            client.DefaultRequestHeaders.Add("referer", $"https://onyolo.com/m/{yoloId}");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36");

            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            paramaters.Add("text", message);
            paramaters.Add("cookie", cookie);
            paramaters.Add("wording", "abcdefg");
            
            StringContent payload = new StringContent(JsonConvert.SerializeObject(paramaters), Encoding.UTF8, "application/json");

            for (int i = 0; i < iterations; i++)
            {
                HttpResponseMessage response = await client.PostAsync(new Uri($"http://onyolo.com/{yoloId}/message"), payload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully send #{i + 1}");
                }
                else
                {
                    Console.WriteLine($"Couldn't send #{i + 1}");
                }
            }
        }
    }
}
