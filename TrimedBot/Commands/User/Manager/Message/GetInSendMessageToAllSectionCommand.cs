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
    public class GetInSendMessageToAllSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        protected BotServices _bot;

        public GetInSendMessageToAllSectionCommand(IServiceProvider provider)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                objectBox.User.UserPlace = UserPlace.Send_Message_ToAll;
                userServices.Update(objectBox.User);
                await userServices.SaveAsync();
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "Send your message:", replyMarkup: Keyboard.CancelKeyboard);
            }
            else
                _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied, replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
