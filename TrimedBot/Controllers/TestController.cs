using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private IServiceProvider provider;

        public TestController(IServiceProvider provider)
        {
            this.provider = provider;
        }

        [Route("sendmessage")]
        public async Task<IActionResult> TestSendMessage()
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.SendTextMessageAsync(326683896, "Test");
            return Content("Message sent");
        }

        [Route("sendertask")]
        public IActionResult TestSenderTask()
        {
            var sender = provider.GetRequiredService<ResponseService>();
            var objectBox = provider.GetRequiredService<ObjectBox>();
            new TextResponseProcessor(objectBox)
            {
                Text = "Test",
                ReceiverId = 326683896
            }.AddThisMessageToService(provider);
            return Content($"Test message added to the service.\nSender state: {sender.State}");
        }

        [Route("database")]
        public IActionResult TestDataBase()
        {
            var ss = provider.GetRequiredService<IUser>();
            return Json(ss.FindAsync(326683896).Result);
        }
    }
}
