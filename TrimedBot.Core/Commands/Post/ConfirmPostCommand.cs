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

namespace TrimedBot.Core.Commands.Post
{
    public class ConfirmPostCommand : ICommand
    {
        private ObjectBox objectBox;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public ConfirmPostCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            List<Processor> messages = new();
            var media = await mediaServices.FindAsync(Guid.Parse(id));
            if (media != null)
            {
                if (!media.IsConfirmed)
                {
                    messages.Add(new VideoResponseProcessor()
                    {
                        ReceiverId = media.User.UserId,
                        Text = $"{media.Title} - {media.Caption}\nThis post confirmed by an admin.",
                        Video = media.FileId
                    });
                    mediaServices.Confirm(media);
                    await mediaServices.SaveAsync();
                }
            }

            await new Classes.Media(objectBox).SendToChannels(media); //Add: Save medias at channels posts table

            messages.Add(new DeleteProcessor()
            {
                UserId = objectBox.User.UserId,
                MessageId = messageId
            });
            var m = await tempMessageServices.FindAsync(objectBox.User.UserId, messageId);
            if (m != null)
            {
                tempMessageServices.Delete(m);
                await tempMessageServices.SaveAsync();
            }
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
