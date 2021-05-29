using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Database.Models;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedCore.Core.Classes;
using Microsoft.Extensions.DependencyInjection;

namespace TrimedBot.Commands.User.Manager
{
    public class AddAdminCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;

        public AddAdminCommand(IServiceProvider provider, string id)
        {
            this.provider = provider;
            this.id = id;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            userServices = provider.GetRequiredService<IUser>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
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
                else await _bot.SendTextMessageAsync(objectBox.User.UserId, "You're manager");
            }
            else await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
