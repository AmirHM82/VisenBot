using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using TrimedBot.Commands.Message;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Post
{
    public class SendSearchedPostsCommand : ICommand
    {
        private IServiceProvider provider;
        protected ITempMessage tempMessageServices;
        protected ObjectBox objectBox;
        protected BotServices _bot;
        private Guid id;

        public SendSearchedPostsCommand(IServiceProvider provider, Guid id)
        {
            this.provider = provider;
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            this.id = id;
        }

        public async Task Do()
        {
            var mediaServices = provider.GetRequiredService<IMedia>();
            var media = await mediaServices.FindAsync(id);
            if (media.User.Id == objectBox.User.Id)
                await new SendPrivateMediaCommand(provider, media).Do();
            else if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
                await new SendPublicMediaCommand(provider, media).Do();
            else
            {
                List<TempMessage> tempMessage = new();
                var sentMessage = await _bot.SendVideoAsync(objectBox.User.UserId, new InputOnlineFile(media.FileId),
                caption: $"{media.Title} - {media.Caption}\nIt's not your post and you aren't admin or manager, So you can't do anything with this post", 
                replyMarkup: objectBox.Keyboard);

                tempMessage.Add(new TempMessage { MessageId = sentMessage.MessageId, UserId = objectBox.User.UserId });
                await tempMessageServices.AddAsync(tempMessage);
                await tempMessageServices.SaveAsync();
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
