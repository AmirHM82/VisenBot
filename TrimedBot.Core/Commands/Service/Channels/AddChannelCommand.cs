using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Channels
{
    public class AddChannelCommand : ICommand
    {
        private ObjectBox objectBox;

        public AddChannelCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            new TextResponseProcessor()
            {
                Text = Sentences.Channel_Add_Guide,
                ReceiverId = objectBox.User.UserId
            }.AddThisMessageToService(objectBox.Provider);

            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
