using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.objectBox.User.Manager.Request
{
    public class AcceptAdminRequestCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public AcceptAdminRequestCommand(IServiceProvider provider, string id, int messageId)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            userServices = provider.GetRequiredService<IUser>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            this.id = id;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var AcceptedUser = await userServices.AcceptAdminRequest(long.Parse(id));

                await _bot.DeleteMessageAsync(objectBox.User.UserId, messageId);
                await tempMessageServices.Delete(objectBox.User.UserId, messageId);
                await tempMessageServices.SaveAsync();
                await _bot.SendTextMessageAsync(AcceptedUser.UserId, "Your admin request accepted. Use /start to see your new keyboard");
            }
            else
                await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
