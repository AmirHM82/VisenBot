using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TrimedBot.ConsoleApp.Request;

namespace TrimedBot.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            //var client = new HttpClient();
            //client.GetAsync("https://localhost:5001/api/TelegramApi/set/");

            // var values = new Dictionary<string, string>
            // {
            //     { "thing1", "hello" },
            //     { "thing2", "world" }
            // };
            // var content = new FormUrlEncodedContent(values);
            // var response = client.PostAsync("http://www.example.com/recepticle.aspx", content);
            //var responseString = await response.Content.ReadAsStringAsync();

            //try
            //{
            //    string data = "{\"update_id\":369291325,\"message\":{\"message_id\":4327,\"from\":{\"id\":326683896,\"is_bot\":false,\"first_name\":\"AmirHM\",\"username\":\"AmirHM000\",\"language_code\":\"en\"},\"date\":1627233412,\"chat\":{\"id\":326683896,\"type\":\"private\",\"username\":\"AmirHM000\",\"first_name\":\"AmirHM\"},\"text\":\"/start\",\"entities\":[{\"type\":\"bot_command\",\"offset\":0,\"length\":6}]}}";
            //    string result = ApiCall.PostApi("https://localhost:5001/api/TelegramApi/new/", data);
            //    Console.WriteLine(result);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            try
            {
                var client = new RestClient("https://localhost:5001/api/telegramapi/new/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/javascript");
                var body = @"""{\""update_id\"":369291325,\""message\"":{\""message_id\"":4327,\""from\"":{\""id\"":326683896,\""is_bot\"":false,\""first_name\"":\""AmirHM\"",\""username\"":\""AmirHM000\"",\""language_code\"":\""en\""},\""date\"":1627233412,\""chat\"":{\""id\"":326683896,\""type\"":\""private\"",\""username\"":\""AmirHM000\"",\""first_name\"":\""AmirHM\""},\""text\"":\""/start\"",\""entities\"":[{\""type\"":\""bot_command\"",\""offset\"":0,\""length\"":6}]}}""";
                request.AddParameter("application/javascript", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
