using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.Admin
{
    public class BanUserCommand : ICommand
    {
        private IServiceProvider provider;
        protected IUser userServices;
        private ObjectBox objectBox;
        private Guid id;

        public BanUserCommand(IServiceProvider provider, Guid id)
        {
            this.provider = provider;
            this.id = id;
            userServices = provider.GetRequiredService<IUser>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                var user = await userServices.FindAsync(id);
                if (!user.IsBanned)
                {
                    user.IsBanned = true;
                    userServices.Update(user);
                    await userServices.SaveAsync();
                }
            }
        }

        public async Task UnDo()
        {
            if (objectBox.User.Access != Access.Member)
            {
                var user = await userServices.FindAsync(id);
                if (user.IsBanned)
                {
                    user.IsBanned = false;
                    userServices.Update(user);
                    await userServices.SaveAsync();
                }
            }
        }
    }
}
