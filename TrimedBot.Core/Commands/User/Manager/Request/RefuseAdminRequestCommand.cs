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
    public class RefuseAdminRequestCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public RefuseAdminRequestCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Manager)
            {
                var RefusedUser = await userServices.RefuseAdminRequest(long.Parse(id));

                messages.Add(new DeleteProcessor()
                {
                    UserId = objectBox.User.UserId,
                    MessageId = messageId
                });
                await tempMessageServices.Delete(objectBox.User.UserId, messageId);
                await tempMessageServices.SaveAsync();
                messages.Add(new TextResponseProcessor()
                {
                    RecieverId = RefusedUser.UserId,
                    Text = "Your request refused"
                });
            }
            else
                messages.Add(new TextResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    Text = Sentences.Access_Denied
                });
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
