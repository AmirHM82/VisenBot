using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Commands.Message
{
    public class NotfoundCommand : ICommand
    {
        private IServiceProvider provider;
        protected BotServices _bot;
        private ObjectBox objectBox;

        public NotfoundCommand(IServiceProvider provider)
        {
            this.provider = provider;
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public Task Do()
        {
            return _bot.SendTextMessageAsync(objectBox.User.UserId, "Command not found.", replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
