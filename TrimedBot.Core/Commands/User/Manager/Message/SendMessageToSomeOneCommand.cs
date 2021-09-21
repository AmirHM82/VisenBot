using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Services;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Telegram.Bot.Types.Message message;

        public SendMessageToSomeOneCommand(ObjectBox objectBox, Telegram.Bot.Types.Message message)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public async Task Do()
        {
            //await new TempMessages(objectBox).Delete();
            message.SendMessage(long.Parse(objectBox.User.Temp), objectBox);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
