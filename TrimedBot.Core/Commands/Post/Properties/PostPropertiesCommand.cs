using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Post.Properties
{
    public class PostPropertiesCommand : ICommand
    {
        private ObjectBox objectBox;
        private Guid postId;

        public PostPropertiesCommand(Guid postId, ObjectBox objectBox)
        {
            this.postId = postId;
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            await new TempMessages(objectBox).Delete();

            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var media = await mediaServices.FindAsync(postId);

            new VideoResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = Keyboard.PostProperties(postId),
                Text = $"{media.Title} - {media.Caption}",
                IsDeletable = true,
                Video = media.FileId
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.Temp = postId.ToString();
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
