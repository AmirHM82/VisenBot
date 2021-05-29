using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.Manager.Message
{
    public class SendMessageToAllCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;
        private string message;

        public SendMessageToAllCommand(IServiceProvider provider, string message)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.message = message;
        }

        public async Task Do()
        {
            var userIds = await userServices.GetUserIds();
            if (userIds.Length != 0)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < userIds.Length; i++)
                {
                    tasks.Add(_bot.SendTextMessageAsync(userIds[i], 
                        $"Message from: {objectBox.User.UserName}({objectBox.User.Access}):\n{message}", ParseMode.Html));
                }
                tasks.Add(_bot.SendTextMessageAsync(objectBox.User.UserId, "Your message sent", replyMarkup: objectBox.Keyboard));
                tasks.Add(userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace }));
                await Task.WhenAll(tasks);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
