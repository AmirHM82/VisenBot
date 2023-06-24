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
    public class ConfirmPostCommand : ICommand
    {
        private ObjectBox objectBox;
        private string id;
        private int messageId;

        // service does not delete message from admin channel
        public ConfirmPostCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            List<Processor> messages = new();
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();

            var media = await mediaServices.FindAsync(Guid.Parse(id));

            //If media is null we should inform the admin for it (in pv
            //And return to stop the rest proccess
            if (media == null)
            {
                messages.Add(new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Media not found",
                });
                return;
            }

            //Inform the admin that the media was confirmd
            //And return to stop the rest proccess
            if (media.IsConfirmed)
            {
                messages.Add(new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "This media is already confirmd"
                });
                return;
            }

            //Create a message to notify user that him/her video is confirmd
            messages.Add(new VideoResponseProcessor(objectBox)
            {
                ReceiverId = media.User.UserId,
                Text = $"{media.Title} - {media.Caption}\nThis post confirmed by an admin.",
                Video = media.FileId
            });
            await mediaServices.Confirm(media);

            //Send the video to channels
            await new Classes.Media(objectBox).SendToOtherChannels(media);

            //Delete the post from admins channel and user's pv
            objectBox.IsNeedDeleteTemps = true;

            //Add created messages to realated service
            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
