using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Member
{
    public class SendAdminRequestCommand : ICommand
    {
        private ObjectBox objectBox;
        //protected IUser userServices;

        public SendAdminRequestCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public async Task Do()
        {
            Processor message;
            if (objectBox.User.Access == Access.Member && objectBox.User.IsSentAdminRequest == false)
            {
                objectBox.User.IsSentAdminRequest = true;
                objectBox.UpdateUserInfo();
                //userServices.SendAdminRequest(objectBox.User);
                //await userServices.SaveAsync();
                message = new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Your admin request sent. Wait for answer from an admin.",
                    Keyboard = objectBox.Keyboard
                };
            }
            else if (objectBox.User.IsSentAdminRequest) message = new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "You have sent an admin request."
            };
            else message = new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = $"You are {objectBox.User.Access}. You don't need to send an admin request."
            };

            message.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
