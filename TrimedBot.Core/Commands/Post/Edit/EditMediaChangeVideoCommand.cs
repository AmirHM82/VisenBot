using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post.Edit
{
    public class EditMediaChangeVideoCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Video video;

        public EditMediaChangeVideoCommand(ObjectBox objectBox, Video video)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.video = video;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            if (video != null)
            {
                await mediaServices.ChangeVideo(Guid.Parse(objectBox.User.Temp), video.FileId);
                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Edited",
                    Keyboard = objectBox.Keyboard
                }.AddThisMessageToService(objectBox.Provider);
                await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Please send a video.",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
