using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
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
                using StreamReader reader = new StreamReader("D:\\Programming\\TrimedBot\\TrimedBot.ConsoleApp\\Json.txt");
                string json = reader.ReadToEnd();
                reader.Close();
                Update u = JsonConvert.DeserializeObject<Update>(json);
                Random r = new Random(int.MaxValue);
                var timer = new Stopwatch();
                timer.Start();
                var client = new RestClient("https://vidsender.ir/api/telegramapi/new/");
                client.Timeout = -1;

                //for (int i = 0; ; i++)
                //{
                    //var id = r.Next();
                    //u.Message.From.Id = id;
                    //u.Message.From.FirstName = "Antar";
                    //u.Message.From.Username = "Antar";
                    //u.Message.Chat.Id = id;
                    //u.Message.Chat.Username = "Antar";
                    //u.Message.Chat.FirstName = "Antar";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.WriteLine($"Id: {id}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Task.Delay(50);

                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    var body = JsonConvert.SerializeObject(u);
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine($"{response.Content}-{timer.Elapsed}");
                //}
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
