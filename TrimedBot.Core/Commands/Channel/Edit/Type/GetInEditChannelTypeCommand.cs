using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Channel.Edit.Type
{
    public class GetInEditChannelTypeCommand : ICommand
    {
        private ObjectBox objectBox;
        private int messageId;
        private int ChannelId;

        public GetInEditChannelTypeCommand(ObjectBox objectBox, int messageId, int channelId)
        {
            this.objectBox = objectBox;
            this.messageId = messageId;
            ChannelId = channelId;
        }

        public Task Do()
        {
            //objectBox.User.UserState = DAL.Enums.UserState.EditChannel_Type; //Not sure this is needed or not
            List<Processor> processList = new List<Processor>();

            if (objectBox.User.Access == DAL.Enums.Access.Manager)
            {
                objectBox.IsNeedDeleteTemps = true;

                processList.AddRange(new Classes.Channel(objectBox).GetTypes(ChannelId));
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
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
