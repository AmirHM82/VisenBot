using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Message
{
    public class StartCommand : ICommand
    {
        protected ObjectBox objectBox;

        public StartCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            new TextResponseProcessor()
            {
                RecieverId = objectBox.User.UserId,
                Text = "Hello, send /help if you need help.",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
