using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Services;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendMessageToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private int messageId;

        public SendMessageToSomeOneCommand(ObjectBox objectBox, int messageId)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.messageId = messageId;
        }

        public async Task Do()
        {
            //new TextResponseProcessor()
            //{
            //    RecieverId = long.Parse(objectBox.User.Temp),
            //    Text = $"Message from: {objectBox.User.UserName}({objectBox.User.Access})\n{message.Text}",
            //    ParseMode = ParseMode.Html
            //}.AddThisMessageToService(objectBox.Provider);

            new ForwardProcessor()
            {
                RecieverId = long.Parse(objectBox.User.Temp),
                FromId = objectBox.User.UserId,
                messageId = messageId
            }.AddThisMessageToService(objectBox.Provider);

            //new TextResponseProcessor()
            //{
            //    RecieverId = objectBox.User.UserId,
            //    Text = "Your message sent",
            //    Keyboard = objectBox.Keyboard
            //}.AddThisMessageToService(objectBox.Provider);

            //await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });            
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
