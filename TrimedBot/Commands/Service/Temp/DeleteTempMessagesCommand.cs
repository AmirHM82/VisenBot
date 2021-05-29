using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Service.Temp
{
    public class DeleteTempMessagesCommand : ICommand
    {
        private IServiceProvider provider;
        protected ITempMessage tempMessageServices;
        protected IUser userServices;
        protected BotServices _bot;
        private ObjectBox objectBox;

        public DeleteTempMessagesCommand(IServiceProvider provider)
        {
            this.provider = provider;
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            var tempMessages = await tempMessageServices.GetAndDeleteAsync(objectBox.User.UserId);
            await tempMessageServices.SaveAsync();
            var tasks = new List<Task>();
            if (tempMessages != null)
            {
                foreach (var item in tempMessages)
                {
                    tasks.Add(_bot.DeleteMessageAsync(item.UserId, item.MessageId));
                }
                try
                {
                    await Task.WhenAll(tasks);
                }
                catch { }
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
