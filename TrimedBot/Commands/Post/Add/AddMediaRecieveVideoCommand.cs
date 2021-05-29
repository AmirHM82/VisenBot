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

namespace TrimedBot.Commands.Post.Add
{
    public class AddMediaRecieveVideoCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected IMedia mediaServices;
        protected BotServices _bot;
        private Video video;

        public AddMediaRecieveVideoCommand(IServiceProvider provider, Video video)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            mediaServices = provider.GetRequiredService<IMedia>();
            _bot = provider.GetRequiredService<BotServices>();
            this.video = video;
        }

        public async Task Do()
        {
            if (video == null)
            {
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Please send a video.", replyMarkup: Keyboard.CancelKeyboard);
            }
            else
            {
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "It's done.", replyMarkup: objectBox.Keyboard);

                string[] TitleCaption = objectBox.User.Temp.Split("*");
                await mediaServices.AddAsync(new Media
                {
                    Title = TitleCaption[0],
                    Caption = TitleCaption[1],
                    FileId = video.FileId,
                    User = objectBox.User,
                    AddDate = DateTime.UtcNow
                });
                await mediaServices.SaveAsync();

                await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
            }
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
