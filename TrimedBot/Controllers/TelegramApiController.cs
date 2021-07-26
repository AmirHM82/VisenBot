using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Services;

namespace TrimedBot.Controllers
{
    [ApiController]
    [Route("api/telegramapi")]
    public class TelegramApiController : Controller
    {
        private IServiceProvider provider;
        private IConfiguration configuration;

        public TelegramApiController(IServiceProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        [Route("set")]
        public async Task<IActionResult> Set()
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.SetWebhookAsync($"{configuration.GetConnectionString("AppHTTPSAddress")}/api/telegramapi/new/{Guid.NewGuid()}");
            return Ok();
        }

        [HttpPost]
        [Route(nameof(New) + "/{id}")]
        public async Task<ActionResult> New(string id)
        {
            Update update = new();
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    update = JsonConvert.DeserializeObject<Update>(await reader.ReadToEndAsync());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return new EmptyResult();
            }
            var updateServices = provider.GetRequiredService<UpdateServices>();
            await updateServices.ProcessUpdate(provider, update);
            return Ok();
        }
        //"{\"update_id\":369291325,\"message\":{\"message_id\":4327,\"from\":{\"id\":326683896,\"is_bot\":false,\"first_name\":\"AmirHM\",\"username\":\"AmirHM000\",\"language_code\":\"en\"},\"date\":1627233412,\"chat\":{\"id\":326683896,\"type\":\"private\",\"username\":\"AmirHM000\",\"first_name\":\"AmirHM\"},\"text\":\"/start\",\"entities\":[{\"type\":\"bot_command\",\"offset\":0,\"length\":6}]}}"
    }
}
