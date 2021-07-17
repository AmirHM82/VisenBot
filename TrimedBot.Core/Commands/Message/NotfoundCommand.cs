using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Message
{
    public class NotfoundCommand : ICommand
    {
        private ObjectBox objectBox;

        public NotfoundCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            new TextResponseProcessor()
            {
                RecieverId = objectBox.User.UserId,
                Text = "Command not found.\nUse your keyboard or send /help",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
