using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Post
{
    public class AddNewPostCommand : ICommand
    {
        private IServiceProvider provider;
        protected BotServices _bot;
        protected ObjectBox objectBox;
        protected IUser userServices;

        public AddNewPostCommand(IServiceProvider provider)
        {
            this.provider = provider;
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
        }

        public async Task Do()
        {
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Send title of the video:", replyMarkup: Keyboard.CancelKeyboard);
            userServices.ChangeUserPlace(objectBox.User, UserPlace.AddMedia_SendTitle);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
