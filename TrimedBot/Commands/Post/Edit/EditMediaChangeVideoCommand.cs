using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Post.Edit
{
    public class EditMediaChangeVideoCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IMedia mediaServices;
        protected IUser userServices;
        protected BotServices _bot;
        private Video video;

        public EditMediaChangeVideoCommand(IServiceProvider provider, Video video)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            mediaServices = provider.GetRequiredService<IMedia>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.video = video;
        }

        public async Task Do()
        {
            if (video != null)
            {
                await mediaServices.ChangeVideo(Guid.Parse(objectBox.User.Temp), video.FileId);
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Edited", replyMarkup: objectBox.Keyboard);
                await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
            }
            else await _bot.SendTextMessageAsync(objectBox.User.UserId, "Please send a video.", replyMarkup: Keyboard.CancelKeyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
