using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post.Delete
{
    public class DeletePostCommand : ICommand
    {
        private ObjectBox objectBox;
        protected ITempMessage tempMessageServices;
        private int messageId;
        private string id;

        public DeletePostCommand(ObjectBox objectBox, int messageId, string id)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            this.messageId = messageId;
            this.id = id;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            if (objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Member || 
                objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Admin || 
                objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Manager)
            {
                List<Processor> messages = new();
                messages.Add(new DeleteProcessor()
                {
                    UserId = objectBox.User.UserId,
                    MessageId = messageId
                });

                var deletedMedia = await mediaServices.Remove(id);
                if (objectBox.User.Access == Access.Admin)
                messages.Add(new VideoResponseProcessor()
                {
                    ReceiverId = deletedMedia.User.UserId,
                    Text = $"{deletedMedia.Title} - {deletedMedia.Caption}\nThis post deleted by an admin.",
                    Video = deletedMedia.FileId
                });
                await mediaServices.SaveAsync();

                new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                await tempMessageServices.Delete(objectBox.User.UserId, messageId);
                await tempMessageServices.SaveAsync();
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
