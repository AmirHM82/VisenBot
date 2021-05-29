using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Post.Add
{
    public class AddMediaRecieveTitleCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;
        private string title;

        public AddMediaRecieveTitleCommand(IServiceProvider provider, string title)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.title = title;
        }

        public async Task Do()
        {
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Please send caption of the video:", replyMarkup: Keyboard.CancelKeyboard);

            objectBox.User.Temp = title;
            objectBox.User.UserPlace = UserPlace.AddMedia_SendCaption;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
