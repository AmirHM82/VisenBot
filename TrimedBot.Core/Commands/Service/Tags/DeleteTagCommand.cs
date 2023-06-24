using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class DeleteTagCommand : ICommand
    {
        private ObjectBox objectBox;
        private int tagId;
        private int messageId;

        public DeleteTagCommand(ObjectBox objectBox, int tagId, int messageId)
        {
            this.objectBox = objectBox;
            this.tagId = tagId;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            var tagsService = objectBox.Provider.GetRequiredService<ITag>();
            var tempService = objectBox.Provider.GetRequiredService<ITempMessage>();

            await tagsService.Delete(tagId);
            await tempService.Delete(objectBox.User.UserId, messageId);

            await tagsService.SaveAsync();
            await tempService.SaveAsync();

            new DeleteProcessor(objectBox)
            {
                UserId = objectBox.User.UserId,
                MessageId = messageId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
