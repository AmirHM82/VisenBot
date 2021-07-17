using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToAllCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string message;

        public SendMessageToAllCommand(ObjectBox objectBox, string message)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.message = message;
        }

        public async Task Do()
        {
            var userIds = await userServices.GetUserIds();
            if (userIds.Length != 0)
            {
                List<Processor> messages = new();
                for (int i = 0; i < userIds.Length; i++)
                {
                    messages.Add(new TextResponseProcessor()
                    {
                        RecieverId = userIds[i],
                        Text = $"Message from: {objectBox.User.UserName}({objectBox.User.Access}):\n{message}",
                        ParseMode = ParseMode.Html
                    });
                }
                messages.Add(new TextResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    Text = "Your message sent",
                    Keyboard = objectBox.Keyboard
                });
                new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
