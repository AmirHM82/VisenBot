using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.User.All
{
    public class OpenSearchInUsersSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;

        public OpenSearchInUsersSectionCommand(IServiceProvider provider)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                await _bot.SendTextMessageAsync(objectBox.User.UserId,
                    "You can find users here with inline mode.",
                    replyMarkup: Keyboard.CancelKeyboard);
                userServices.ChangeUserPlace(objectBox.User, UserPlace.Search_Users);
                await userServices.SaveAsync();
            }
            else await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied, replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
