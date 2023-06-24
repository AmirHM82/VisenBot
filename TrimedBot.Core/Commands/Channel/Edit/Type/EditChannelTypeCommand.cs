using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Commands.Channel.Edit.Type
{
    public class EditChannelTypeCommand : ICommand
    {
        private ChannelType channelType;
        private int channelId;
        private int messageId;
        private ObjectBox objectBox;

        public EditChannelTypeCommand(ChannelType channelType, int channelId, ObjectBox objectBox, int messageId)
        {
            this.channelType = channelType;
            this.channelId = channelId;
            this.objectBox = objectBox;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            await channelService.ChangeType(channelId, channelType);

            new MultiProcessor(await new Classes.Channel(objectBox).GetMessage(channelId), objectBox)
                .AddThisMessageToService(objectBox.Provider);

            objectBox.IsNeedDeleteTemps = true;
            objectBox.User.Temp = messageId.ToString();
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
