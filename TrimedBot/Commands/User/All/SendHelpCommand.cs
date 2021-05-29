using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.All
{
    public class SendHelpCommand : ICommand
    {
        private ObjectBox objectBox;
        protected BotServices _bot;

        public SendHelpCommand(IServiceProvider provider)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
        }

        public async Task Do()
        {
            switch (objectBox.User.Access)
            {
                case Access.Manager:
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Help_Manager, replyMarkup: objectBox.Keyboard);
                    break;
                case Access.Admin:
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Help_Admin, replyMarkup: objectBox.Keyboard);
                    break;
                case Access.Member:
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Help_Member, replyMarkup: objectBox.Keyboard);
                    break;
                default:
                    break;
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
