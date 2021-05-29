using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Services;
using TrimedBot.Core.Interfaces;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.Manager.Message
{
    public class SendMessageToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        private string message;

        public SendMessageToSomeOneCommand(IServiceProvider provider, string message)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            userServices = provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public async Task Do()
        {
            var tasks = new List<Task>();
            tasks.Add(_bot.SendTextMessageAsync(long.Parse(objectBox.User.Temp), $"Message from: {objectBox.User.UserName}({objectBox.User.Access})\n{message}", ParseMode.Html));
            tasks.Add(_bot.SendTextMessageAsync(objectBox.User.UserId, "Your message sent", replyMarkup: objectBox.Keyboard));
            tasks.Add(userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace }));
            await Task.WhenAll(tasks);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
