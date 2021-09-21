using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Message
{
    public class PingCommand : ICommand
    {
        private ObjectBox objectBox;

        public PingCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            new TextResponseProcessor()
            {
                ReceiverId = objectBox.ChatId,
                Text = "pong"
            }.AddThisMessageToService(objectBox.Provider);

            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
