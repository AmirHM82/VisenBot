using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Post.Edit
{
    public class GetInEditMediaChangeVideoSectionCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        private string id;

        public GetInEditMediaChangeVideoSectionCommand(IServiceProvider provider, string id)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            this.id = id;
        }

        public async Task Do()
        {
            objectBox.User.UserPlace = UserPlace.EditMedia_Video;
            objectBox.User.Temp = id;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Send new video:", replyMarkup: Keyboard.CancelKeyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
