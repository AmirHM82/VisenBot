using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.Member
{
    public class SendAdminRequestCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;

        public SendAdminRequestCommand(IServiceProvider provider)
        {
            userServices = provider.GetRequiredService<IUser>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Member && objectBox.User.IsSentAdminRequest == false)
            {
                userServices.SendAdminRequest(objectBox.User);
                await userServices.SaveAsync();
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Your admin request sent. Wait for answer from an admin.", replyMarkup: objectBox.Keyboard);
            }
            else if (objectBox.User.IsSentAdminRequest) await _bot.SendTextMessageAsync(objectBox.User.UserId, "You have sent an admin request.");
            else await _bot.SendTextMessageAsync(objectBox.User.UserId, $"You are {objectBox.User.Access}. You don't need to send an admin request.");
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
