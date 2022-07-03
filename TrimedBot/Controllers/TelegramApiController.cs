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
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Context;

namespace TrimedBot.Controllers
{
    [Route("telegram")]
    public class TelegramApiController : Controller
    {
        private IServiceProvider provider;
        private IConfiguration configuration;
        private DB db;

        public TelegramApiController(IServiceProvider provider, IConfiguration configuration, DB db)
        {
            this.provider = provider;
            this.configuration = configuration;
            this.db = db;
        }

        [Route("webhook/set/{token}")]
        public async Task<IActionResult> SetWebhook(string token)
        {
            var bot = provider.GetRequiredService<BotServices>();
            try
            {
                await bot.SetWebhookAsync($"{configuration["AppHTTPSAddress"]}/telegram/update/new/{token}");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return Content("url is: " + (await bot.GetWebhookInfoAsync()).Url);
        }

        [Route("webhook/delete")]
        public async Task<IActionResult> DeleteWebhook()
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.DeleteWebhookAsync();
            return Ok();
        }

        [HttpPost]
        [Route("update/new/{token}")]
        //[Route("update/new")]
        public async Task<IActionResult> New(string token, Update update)
        {
            if (token == configuration["Token"])
            {
                try
                {
                    using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                    {
                        update = JsonConvert.DeserializeObject<Update>(await reader.ReadToEndAsync());
                    }
                }
                catch (Exception e)
                {
                    return Content($"Error: {e.Message}");
                }
                var updateServices = provider.GetRequiredService<UpdateServices>();
                await updateServices.ProcessUpdate(provider, update);
                return Ok();
            }
            else return Content("Token is not true.");
        }

        [Route("update/fake")]
        public IActionResult FakeUpdate()
        {
            var botService = provider.GetRequiredService<BotServices>();

            List<Update> updates = new();
            updates.Add(new Update()
            {
                InlineQuery = new InlineQuery()
                {
                    Id = "1",
                    Query = "aa",
                    From = new User()
                    {
                        Id = 326683896,
                        Username = "DarknessMaster0",
                        FirstName = "AmirHM",
                        LastName = null
                    }
                }
            });
            updates.Add(new Update()
            {
                InlineQuery = new InlineQuery()
                {
                    Id = "3",
                    Query = "ss",
                    From = new User()
                    {
                        Id = 1,
                        Username = "Dark",
                        FirstName = "Amir",
                        LastName = null
                    }
                }
            });

            foreach (var update in updates)
            {
                botService.Handle(update);
            }

            return Ok();
        }
    }
}
