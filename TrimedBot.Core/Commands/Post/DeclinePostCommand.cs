using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InputFiles;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using Telegram.Bot.Types.Enums;

namespace TrimedBot.Core.Commands.Post
{
    public class DeclinePostCommand : ICommand
    {
        private ObjectBox objectBox;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public DeclinePostCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do() //Move this class to ConfirmPostCommand.cs (Undo method)
        {
            List<Processor> messages = new();
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            {
                var media = await mediaServices.FindAsync(Guid.Parse(id));

                //If media is null we should inform the admin for it (in pv)
                //And return to stop the rest proccess
                if (media == null)
                {
                    messages.Add(new TextResponseProcessor(objectBox)
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Media not found"
                    });
                    return;
                }

                if (media.IsConfirmed)
                {
                    messages.Add(new VideoResponseProcessor(objectBox)
                    {
                        ReceiverId = media.User.UserId,
                        Text = $"{media.Title} - {media.Caption}\nThis post declined by an admin.",
                        Video = media.FileId
                    });
                    mediaServices.Decline(media);
                    await mediaServices.SaveAsync();

                    //Add: Delete (or edit) the post from channel and channels posts table
                    messages.AddRange(await new ChannelPosts(objectBox).Delete(media.Id));
                }

                //Create a Delete proccess message to delete the message in pv
                if (objectBox.ChatType is ChatType.Private)
                    messages.Add(new DeleteProcessor(objectBox)
                    {
                        UserId = objectBox.ChatId,
                        MessageId = messageId
                    });

                objectBox.IsNeedDeleteTemps = true;
            }

            //Send the created messages to realted service
            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
