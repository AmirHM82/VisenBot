using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Post.Tag
{
    public class DeletePostsTagCommand : ICommand
    {
        private ObjectBox objectBox;
        private int tagId;
        private int messageId;

        public DeletePostsTagCommand(ObjectBox objectBox, int tagId, int messageId)
        {
            this.objectBox = objectBox;
            this.tagId = tagId;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            var mediaService = objectBox.Provider.GetRequiredService<IMedia>();
            await mediaService.RemoveTag(Guid.Parse(objectBox.User.Temp), tagId);
            new DeleteProcessor()
            {
                MessageId = messageId,
                UserId = objectBox.ChatId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
