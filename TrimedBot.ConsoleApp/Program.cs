using System;
using System.Net.Http;
using TrimedBot.ConsoleApp.Request;

namespace TrimedBot.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App is ready.");
            while (true)
            {
                Console.Write("=>");
                string command = Console.ReadLine();
                Response(command);
            }
        }

        public static string MainUrl = "https://localhost:5001/api";
        static void Response(string command)
        {
            switch (command)
            {
                case "Start recieving":
                    break;
                case "Stop recieving":
                    break;
                case "Start responsing":
                    ApiCall.GetApi($"{MainUrl}/responseservice/StartResponsing");
                    break;
                case "Stop responsing":
                    HttpClient client = new();
                    using (var response = client.Send(new HttpRequestMessage(HttpMethod.Get, $"{MainUrl}/responseservice/StopResponsing")))
                    {
                        Console.WriteLine("Response: {0}", response.StatusCode);
                    }
                    //ApiCall.GetApi("http://localhost:5001/api/response/StopResponsing");
                    break;
                default:
                    break;
            }
        }
    }
}
