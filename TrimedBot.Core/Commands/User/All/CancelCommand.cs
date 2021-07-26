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

namespace TrimedBot.Core.Commands.User.All
{
    public class CancelCommand : ICommand
    {
        protected IUser userServices;
        protected ObjectBox objectBox;

        public CancelCommand(ObjectBox objectBox)
        {
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Canceled",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserPlace = UserPlace.NoWhere;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
