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

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                objectBox.User.UserPlace = UserPlace.Send_Message_ToAll;
                //userServices.Update(objectBox.User);
                //await userServices.SaveAsync();
                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Send your message:",
                    Keyboard = Keyboard.CancelKeyboard()
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied,
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
