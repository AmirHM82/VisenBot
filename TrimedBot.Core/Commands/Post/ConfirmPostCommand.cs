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
            List<Processor> messages = new();
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();

            var media = await mediaServices.FindAsync(Guid.Parse(id));
            if (media != null)
            {
                if (media.IsConfirmed) return;
                messages.Add(new VideoResponseProcessor()
                {
                    ReceiverId = media.User.UserId,
                    Text = $"{media.Title} - {media.Caption}\nThis post confirmed by an admin.",
                    Video = media.FileId
                });
                await mediaServices.Confirm(media);

                await new Classes.Media(objectBox).SendToChannels(media); //Add: Save medias at channels posts table (Check is media tracking?)
            }

            messages.Add(new DeleteProcessor()
            {
                UserId = objectBox.User.UserId,
                MessageId = messageId
            });
            await tempMessageServices.Delete(objectBox.User.UserId, messageId);
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
