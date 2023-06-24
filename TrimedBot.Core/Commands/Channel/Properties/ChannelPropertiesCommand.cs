using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Services;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Channel.Properties
{
    public class ChannelPropertiesCommand : ICommand
    {
        private int messageId;
        private ObjectBox objectBox;
        private int ChannelId;

        public ChannelPropertiesCommand(ObjectBox objectBox, int channelId, int messageId)
        {
            this.objectBox = objectBox;
            this.messageId = messageId;
            ChannelId = channelId;
        }

        public async Task Do()
        {
            List<Processor> processList = new List<Processor>();

            if (objectBox.User.Access == DAL.Enums.Access.Manager)
            {
                objectBox.IsNeedDeleteTemps = true;

                processList.AddRange(await new Classes.Channel(objectBox).GetMessage(ChannelId));
            }
            else processList.Add(new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = objectBox.Keyboard,
                Text = Sentences.Access_Denied
            });
            new MultiProcessor(processList, objectBox).AddThisMessageToService(objectBox.Provider);

            objectBox.User.Temp = messageId.ToString();
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
