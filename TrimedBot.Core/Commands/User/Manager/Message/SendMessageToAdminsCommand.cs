using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToAdminsCommand : ICommand
    {
        private ObjectBox objectBox;
        private Telegram.Bot.Types.Message message;

        public SendMessageToAdminsCommand(ObjectBox objectBox, Telegram.Bot.Types.Message message)
        {
            this.objectBox = objectBox;
            this.message = message;
        }


        public async Task Do()
        {
            var userServices = objectBox.Provider.GetRequiredService<IUser>();
            var admins = await userServices.GetAllAdminsAsync();
            if (admins.Count > 0)
            {
                foreach (var admin in admins)
                {
                    message.SendMessage(admin.UserId, objectBox);
                }
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
