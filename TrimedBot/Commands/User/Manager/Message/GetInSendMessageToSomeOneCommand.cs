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

namespace TrimedBot.Commands.User.Manager.Message
{
    public class GetInSendMessageToSomeOneCommand : ICommand
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        protected BotServices _bot;
        protected IUser userServices;
        private string userId;

        public GetInSendMessageToSomeOneCommand(IServiceProvider provider, string userId)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            userServices = provider.GetRequiredService<IUser>();
            this.userId = userId;
        }

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                objectBox.User.UserPlace = UserPlace.Send_Message_ToSomeone;
                objectBox.User.Temp = userId;
                userServices.Update(objectBox.User);
                await userServices.SaveAsync();
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Send your message:", replyMarkup: Keyboard.CancelKeyboard);
            }
            else
                await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied, replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
