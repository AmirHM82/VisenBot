using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToAllCommand : ICommand
    {
        private ObjectBox objectBox;
        private Telegram.Bot.Types.Message message;

        public SendMessageToAllCommand(ObjectBox objectBox, Telegram.Bot.Types.Message message)
        {
            this.objectBox = objectBox;
            this.message = message;
        }

        public async Task Do()
        {
            var userServices = objectBox.Provider.GetRequiredService<IUser>();
            var userIds = await userServices.GetUserIds();
            if (userIds.Length != 0)
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    message.SendMessage(userIds[i], objectBox);
                }
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
