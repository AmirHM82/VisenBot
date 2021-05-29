using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Commands.Message
{
    public class StartCommand : ICommand
    {
        private IServiceProvider provider;
        protected BotServices _bot;
        protected ObjectBox objectBox;

        public StartCommand(IServiceProvider provider)
        {
            this.provider = provider;
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Hello, send /help if you need help.", replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
