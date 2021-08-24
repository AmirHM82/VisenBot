using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.All
{
    public class OpenSearchInUsersSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        //protected IUser userServices;

        public OpenSearchInUsersSectionCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "You can find users here with inline mode.",
                    Keyboard = Keyboard.CancelKeyboard()
                }.AddThisMessageToService(objectBox.Provider);

                objectBox.User.UserLocation = UserLocation.Search_Users;
                objectBox.UpdateUserInfo();
                //userServices.ChangeUserPlace(objectBox.User, UserPlace.Search_Users);
                //await userServices.SaveAsync();
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied,
                Keyboard = objectBox.Keyboard
            };
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
