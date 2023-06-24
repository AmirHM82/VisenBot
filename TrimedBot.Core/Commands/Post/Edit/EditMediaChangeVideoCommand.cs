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
using TrimedBot.Core.Classes.Processors;

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
            List<Processor> processes = new();

            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            if (video != null)
            {
                var media = await mediaServices.ChangeVideo(Guid.Parse(objectBox.User.Temp), video.FileId);

                processes.Add(new TextResponseProcessor(objectBox)
                {
                    Keyboard = objectBox.Keyboard,
                    ReceiverId = objectBox.User.UserId,
                    Text = "Edited"
                });

                var state = objectBox.User.LastUserState;
                processes.Add(new VideoResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Keyboard = state == UserState.SeePublicAddedVideos ? Keyboard.PublicPostProperties(media.Id, true) : Keyboard.PrivatePostProperties(media.Id, true),
                    Text = $"{media.Title} - {media.Caption}",
                    Video = media.FileId,
                    IsDeletable = true
                });

                await new Classes.Media(objectBox).SendToAdminChannels(media);

                objectBox.IsNeedDeleteTemps = true;
                objectBox.User.Temp = null;
                objectBox.User.UserState = UserState.NoWhere;
                objectBox.UpdateUserInfo();
            }
            else processes.Add(new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Please send a video.",
                Keyboard = Keyboard.CancelKeyboard()
            });

            new MultiProcessor(processes, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
