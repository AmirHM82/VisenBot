using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Commands.Message
{
    public class SendNPMessageCommand : ICommand
    {
        private int pageNumber;
        private string Category;
        protected BotServices _bot;
        protected ITempMessage tempMessageServices;
        private ObjectBox objectBox;

        public SendNPMessageCommand(IServiceProvider provider, int pageNumber, string category)
        {
            this.pageNumber = pageNumber;
            Category = category;
            _bot = provider.GetRequiredService<BotServices>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            objectBox = provider.GetRequiredService<ObjectBox>(); ;
        }

        public async Task Do()
        {
            int nextPage = pageNumber + 1;
            int previousPage = pageNumber - 1;

            InlineKeyboardButton[] t2 =
            {
                InlineKeyboardButton.WithCallbackData("Next", $"{Category}/Next/{nextPage}/"),
                InlineKeyboardButton.WithCallbackData("Previous", $"{Category}/Previous/{previousPage}/")
            };

            var message = await _bot.SendTextMessageAsync(objectBox.User.UserId, $"Page: {pageNumber}", replyMarkup: new InlineKeyboardMarkup(t2));
            var message1 = await _bot.SendTextMessageAsync(objectBox.User.UserId, "Here you are.", replyMarkup: Keyboard.CancelKeyboard);
            var tempMessages = new List<TempMessage>();
            tempMessages.Add(new TempMessage { MessageId = message.MessageId, UserId = objectBox.User.UserId });
            tempMessages.Add(new TempMessage { MessageId = message1.MessageId, UserId = objectBox.User.UserId });
            await tempMessageServices.AddAsync(tempMessages);
            await tempMessageServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
