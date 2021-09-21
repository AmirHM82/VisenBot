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

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class GetInSendMessageToAllSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        //protected IUser userServices;

        public GetInSendMessageToAllSectionCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                objectBox.User.UserState = UserState.Send_Message_ToAll;
                objectBox.UpdateUserInfo();

                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Send your messages:",
                    Keyboard = Keyboard.CancelKeyboard()
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied,
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
