using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Post.Delete
{
    public class DeletePostCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IMedia mediaServices;
        protected ITempMessage tempMessageServices;
        private int messageId;
        private string id;

        public DeletePostCommand(IServiceProvider provider, int messageId, string id)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            mediaServices = provider.GetRequiredService<IMedia>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            this.messageId = messageId;
            this.id = id;
        }

        public async Task Do()
        {
            if (objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Member || 
                objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Admin || 
                objectBox.User.UserPlace == UserPlace.SeeAddedVideos_Manager)
            {
                try
                {
                    await _bot.DeleteMessageAsync(objectBox.User.UserId, messageId);
                }
                catch (/*ApiRequest*/Exception) { }

                var deletedMedia = await mediaServices.Remove(id);
                if (objectBox.User.Access == Access.Admin)
                    await _bot.SendVideoAsync(deletedMedia.User.UserId, 
                        new InputOnlineFile(deletedMedia.FileId), 
                        caption: $"{deletedMedia.Title} - {deletedMedia.Caption}\nThis post deleted by an admin.");
                await mediaServices.SaveAsync();

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
