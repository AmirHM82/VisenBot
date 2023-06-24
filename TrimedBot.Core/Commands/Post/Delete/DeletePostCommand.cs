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
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes;

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

            List<Processor> messages = new();

            //Create a Delete proccess message to delete the message in pv
            if (objectBox.ChatType == ChatType.Private)
                messages.Add(new DeleteProcessor(objectBox)
                {
                    UserId = objectBox.User.UserId,
                    MessageId = messageId
                });

            //Remove video from db
            var deletedMedia = await mediaServices.Remove(id);

            //Delete post from channels
            messages.AddRange(await new ChannelPosts(objectBox).Delete(deletedMedia.Id));

            //Create a message for the user who has added the video
            messages.Add(new VideoResponseProcessor(objectBox)
            {
                ReceiverId = deletedMedia.User.UserId,
                Text = $"{deletedMedia.Title} - {deletedMedia.Caption}\nThis post deleted by an admin.",
                Video = deletedMedia.FileId
            });

            await mediaServices.SaveAsync();

            //Send the created messages to realted service
            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);

            //U messed up again. It deletes all the messages from admin channel!!!!
            //objectBox.IsNeedDeleteTemps = true; //It deletes admin post messages from channel post table!!!!!!!
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
