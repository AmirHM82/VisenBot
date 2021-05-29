using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Database.Models;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.User.Manager.Request
{
    public class DeleteAdminCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        private string id;
        private int messageId;

        public DeleteAdminCommand(IServiceProvider provider, string id, int messageId)
        {
            this.provider = provider;
            this.id = id;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            userServices = provider.GetRequiredService<IUser>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            this.messageId = messageId;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var tasksa = new List<Task>();
                var dadmin = await userServices.FindAsync(Guid.Parse(id));
                tasksa.Add(_bot.SendTextMessageAsync(dadmin.UserId, "Manager deleted you from admins.", 
                    replyMarkup: Keyboard.StartKeyboard_Member));
                dadmin.Access = Access.Member;
                userServices.Update(dadmin);
                tasksa.Add(userServices.SaveAsync());

                try
                {
                    tasksa.Add(_bot.DeleteMessageAsync(objectBox.User.UserId, messageId));
                }
                catch (Exception) { }
                tasksa.Add(tempMessageServices.Delete(objectBox.User.UserId, messageId));
                tasksa.Add(tempMessageServices.SaveAsync());
                await Task.WhenAll(tasksa);
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
