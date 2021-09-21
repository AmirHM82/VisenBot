using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Channels
{
    public class DeleteChannelCommand : ICommand
    {
        private ObjectBox objectBox;
        private int channelId;
        private int messageId;

        public DeleteChannelCommand(ObjectBox objectBox, int channelId, int messageId)
        {
            this.objectBox = objectBox;
            this.channelId = channelId;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channel = await channelService.FindAsync(channelId);
            channelService.Delete(channel);
            await channelService.SaveAsync();

            var tempService = objectBox.Provider.GetRequiredService<ITempMessage>();
            await tempService.Delete(objectBox.User.UserId, messageId);

            new DeleteProcessor()
            {
                MessageId = messageId,
                UserId = objectBox.User.UserId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
