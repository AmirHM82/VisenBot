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

        public async Task Do()
        {
            List<Processor> messages = new();
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>(); ;
            if (objectBox.User.UserLocation == UserLocation.SeeAddedVideos_Admin ||
                objectBox.User.UserLocation == UserLocation.SeeAddedVideos_Manager ||
                objectBox.User.UserLocation == UserLocation.Search_Posts)
            {
                var media = await mediaServices.FindAsync(Guid.Parse(id));
                if (media != null)
                {
                    if (media.IsConfirmed)
                    {
                        messages.Add(new VideoResponseProcessor()
                        {
                            ReceiverId = media.User.UserId,
                            Text = $"{media.Title} - {media.Caption}\nThis post declined by an admin.",
                            Video = media.FileId
                        });
                        mediaServices.Decline(media);
                        await mediaServices.SaveAsync();
                    }
                }

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
            }
            else
                messages.Add(new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = Sentences.Access_Denied
                });
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
