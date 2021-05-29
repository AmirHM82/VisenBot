using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.All
{
    public class CancelCommand : ICommand
    {
        private IServiceProvider provider;
        protected IUser userServices;
        protected BotServices _bot;
        protected ObjectBox objectBox;

        public CancelCommand(IServiceProvider provider)
        {
            this.provider = provider;
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Canceled", replyMarkup: objectBox.Keyboard);
            objectBox.User.UserPlace = UserPlace.NoWhere;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
