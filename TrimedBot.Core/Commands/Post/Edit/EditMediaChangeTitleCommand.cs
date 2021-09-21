using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;

namespace TrimedBot.Core.Commands.Post.Edit
{
    public class EditMediaChangeTitleCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string newTitle;

        public EditMediaChangeTitleCommand(ObjectBox objectBox, string newTitle)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.newTitle = newTitle;
        }

        public async Task Do()
        {
            List<Processor> processes = new();

            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var media = await mediaServices.ChangeTitle(Guid.Parse(objectBox.User.Temp), newTitle);

            processes.Add(new TextResponseProcessor()
            {
                Keyboard = objectBox.Keyboard,
                ReceiverId = objectBox.User.UserId,
                Text = "Edited"
            });

            var state = objectBox.User.LastUserState;
            processes.Add(new VideoResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = state == UserState.SeePublicAddedVideos ? Keyboard.PublicPostProperties(media.Id, true) : Keyboard.PrivatePostProperties(media.Id, true),
                Text = $"{media.Title} - {media.Caption}",
                Video = media.FileId,
                IsDeletable = true
            });

            objectBox.IsNeedDeleteTemps = true;
            objectBox.User.Temp = null;
            objectBox.User.UserState = UserState.NoWhere;
            objectBox.UpdateUserInfo();

            new MultiProcessor(processes).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
