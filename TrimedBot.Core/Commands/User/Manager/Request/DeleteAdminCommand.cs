using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class DeleteAdminCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public DeleteAdminCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            this.id = id;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            this.messageId = messageId;
        }

        public async Task Do()
        {
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Manager)
            {
                var dadmin = await userServices.FindAsync(Guid.Parse(id));
                messages.Add(new TextResponseProcessor()
                {
                    RecieverId = dadmin.UserId,
                    Text = "Manager deleted you from admins.",
                    Keyboard = Keyboard.StartKeyboard_Member()
                });
                dadmin.Access = Access.Member;
                userServices.Update(dadmin);
                await userServices.SaveAsync();

                    messages.Add(new DeleteProcessor()
                    {
                        UserId = objectBox.User.UserId,
                        MessageId = messageId
                    });
                await tempMessageServices.Delete(objectBox.User.UserId, messageId);
                await tempMessageServices.SaveAsync();
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
