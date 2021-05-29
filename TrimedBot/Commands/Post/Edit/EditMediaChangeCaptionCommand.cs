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
    public class EditMediaChangeCaptionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IMedia mediaServices;
        protected IUser userServices;
        protected BotServices _bot;
        private string newCaption;

        public EditMediaChangeCaptionCommand(IServiceProvider provider, string newCaption)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            mediaServices = provider.GetRequiredService<IMedia>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.newCaption = newCaption;
        }

        public async Task Do()
        {
            await mediaServices.ChangeCaption(Guid.Parse(objectBox.User.Temp), newCaption);
            await _bot.SendTextMessageAsync(objectBox.User.UserId, "Edited", replyMarkup: objectBox.Keyboard);
            await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
