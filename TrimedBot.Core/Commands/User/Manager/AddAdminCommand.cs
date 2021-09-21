using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.DAL.Enums;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager
{
    public class AddAdminCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;

        public AddAdminCommand(ObjectBox objectBox, string id)
        {
            this.objectBox = objectBox;
            this.id = id;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var selectedUser = await userServices.FindAsync(Guid.Parse(id));
                if (selectedUser.Access != Access.Manager)
                {
                    selectedUser.Access = Access.Admin;
                    userServices.Update(selectedUser);
                    await userServices.SaveAsync();
                }
                else new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "You're manager"
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
