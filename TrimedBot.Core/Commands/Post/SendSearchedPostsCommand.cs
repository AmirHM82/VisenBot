using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using TrimedBot.Core.Commands.Message;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post
{
    public class SendSearchedPostsCommand : ICommand
    {
        protected ObjectBox objectBox;
        private Guid id;

        public SendSearchedPostsCommand(ObjectBox objectBox, Guid id)
        {
            this.objectBox = objectBox;
            this.id = id;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var media = await mediaServices.FindAsync(id);
            if (media.User.Id == objectBox.User.Id)
                await new SendPrivateMediaCommand(objectBox, media).Do();
            else if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
                await new SendPublicMediaCommand(objectBox, media).Do();
            else
            {
                new VideoResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    FileId = media.FileId,
                    Text = $"{media.Title} - {media.Caption}\nIt's not your post and you aren't admin or manager, So you can't do anything with this post",
                    Keyboard = objectBox.Keyboard,
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
