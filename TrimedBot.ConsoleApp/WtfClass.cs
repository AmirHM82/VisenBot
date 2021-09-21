using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Responses;
using TrimedBot.Core.Classes.Responses.ResponseTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Context;

namespace TrimedBot.ConsoleApp
{
    public class WtfClass
    {
        public async Task Do()
        {
            var options = new DbContextOptions<DB>() { };
            var db = new DB(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(DB)))
                .Returns(db);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(ResponseService)))
                .Returns(new ResponseService(serviceProvider.Object));

            var user = new DAL.Entities.User()
            {
                UserId = 326683896
            };
            var objectBox = new ObjectBox(serviceProvider.Object);
            objectBox.User = user;
            objectBox.Keyboard = Keyboard.StartKeyboard_Member();
            objectBox.ChatId = 326683896;
            objectBox.Settings = new DAL.Entities.Settings()
            {
                IsResponsingAvailable = true
            };

            serviceProvider
                .Setup(x => x.GetService(typeof(ITempMessage)))
                .Returns(new TempMessageServices(db));

            var message = new Message()
            {
                Text = "/start"
            };

            Input messageInput = new MessageInput(objectBox, message);
            await messageInput.Response();
        }

        public void SendUpdateTest()
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.White;
                Task.Delay(50);

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(u);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine($"{response.Content}-{timer.Elapsed}");
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
