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
    public class AddMediaRecieveCaptionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;
        private string Caption;

        public AddMediaRecieveCaptionCommand(IServiceProvider provider, string caption)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            Caption = caption;
        }

        public async Task Do()
        {
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Now it's time to send the video:", replyMarkup: Keyboard.CancelKeyboard);

            objectBox.User.Temp = $"{objectBox.User.Temp}*{Caption}";
            objectBox.User.UserPlace = UserPlace.AddMedia_SendMedia;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
