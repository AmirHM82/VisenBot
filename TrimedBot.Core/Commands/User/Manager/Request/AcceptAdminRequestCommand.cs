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
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.objectBox.User.Manager.Request
{
    public class AcceptAdminRequestCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public AcceptAdminRequestCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Manager)
            {
                var AcceptedUser = await userServices.AcceptAdminRequest(long.Parse(id));

                messages.Add(new DeleteProcessor()
                {
                    UserId = objectBox.User.UserId,
                    MessageId = messageId
                });
                await tempMessageServices.Delete(objectBox.User.UserId, messageId);
                await tempMessageServices.SaveAsync();
                messages.Add(new TextResponseProcessor()
                {
                    ReceiverId = AcceptedUser.UserId,
                    Text = Sentences.Admin_Request_Accepted,
                    Keyboard = Keyboard.StartKeyboard_Admin()
                });
            }
            else
                messages.Add(new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = Sentences.Access_Denied
                });
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
