using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Post.Edit
{
    public class EditMediaChangeTitleCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IMedia mediaServices;
        protected IUser userServices;
        protected BotServices _bot;
        private string newTitle;

        public EditMediaChangeTitleCommand(IServiceProvider provider, string newTitle)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            mediaServices = provider.GetRequiredService<IMedia>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.newTitle = newTitle;
        }

        public async Task Do()
        {
            await mediaServices.ChangeTitle(Guid.Parse(objectBox.User.Temp), newTitle);

            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Edited", replyMarkup: objectBox.Keyboard);

            await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
