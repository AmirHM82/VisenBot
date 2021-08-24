using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;
using Telegram.Bot.Types;

namespace TrimedBot.Core.Commands.User.All
{
    public class CancelCommand : ICommand
    {
        protected ObjectBox objectBox;

        public CancelCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            if (objectBox.User.UserLocation != UserLocation.NoWhere)
            {
                await new TempMessages(objectBox).Delete();

                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Canceled",
                    Keyboard = objectBox.Keyboard
                }.AddThisMessageToService(objectBox.Provider);

                objectBox.User.UserLocation = UserLocation.NoWhere;
                objectBox.UpdateUserInfo();
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "You are in home page.",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
