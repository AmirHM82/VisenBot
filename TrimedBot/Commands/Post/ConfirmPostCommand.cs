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
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Post
{
    public class ConfirmPostCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IMedia mediaServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public ConfirmPostCommand(IServiceProvider provider, string id, int messageId)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            mediaServices = provider.GetRequiredService<IMedia>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            if (objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Admin || objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Manager || objectBox.User.UserPlace == UserPlace.Search_Posts)
            {
                var media = await mediaServices.FindAsync(Guid.Parse(id));
                if (media != null)
                {
                    if (!media.IsConfirmed)
                    {
                        await _bot.SendVideoAsync(media.User.UserId, new InputOnlineFile(media.FileId),
                            caption: $"{media.Title} - {media.Caption}\nThis post confirmed by an admin.");
                        mediaServices.Confirm(media);
                        await mediaServices.SaveAsync();
                    }
                }

                try
                {
                    await _bot.DeleteMessageAsync(objectBox.User.UserId, messageId);
                }
                catch (ApiRequestException) { }
                var m = await tempMessageServices.FindAsync(objectBox.User.UserId, messageId);
                if (m != null)
                {
                    tempMessageServices.Delete(m);
                    await tempMessageServices.SaveAsync();
                }
            }
            else
                await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
