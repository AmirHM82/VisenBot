using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.User.All
{
    public class ChosenInlineSearchInUsersCommand : ICommand
    {
        private IServiceProvider provider;
        protected IUser userServices;
        protected ITempMessage tempMessageServices;
        protected BotServices _bot;
        protected ObjectBox objectBox;
        private ChosenInlineResult result;

        public ChosenInlineSearchInUsersCommand(IServiceProvider provider, ChosenInlineResult result)
        {
            this.provider = provider;
            userServices = provider.GetRequiredService<IUser>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
            this.result = result;
        }

        public async Task Do()
        {
            var selectedUser = await userServices.FindAsync(long.Parse(result.ResultId));

            if (selectedUser != null)
            {
                if (selectedUser != null)
                {
                    InlineKeyboardButton[] t1 =
                    {
                    selectedUser.Access == Access.Member ?
                    InlineKeyboardButton.WithCallbackData("Make admin", $"Admin/Add/{selectedUser.Id}") :
                    selectedUser.Access == Access.Admin ?
                    InlineKeyboardButton.WithCallbackData("Delete admin", $"Admin/Delete/{selectedUser.Id}") :
                    InlineKeyboardButton.WithCallbackData("Delete manager", $"Manager/Delete/{selectedUser.Id}")
                    };

                    //I didn't write callbacks of this section

                    InlineKeyboardButton[] t2 =
                    {
                    InlineKeyboardButton.WithCallbackData("Send a message", $"User/Send/Message/{selectedUser.UserId}")
                    };

                    InlineKeyboardButton[] t3 =
                    {
                    selectedUser.IsBanned ?
                    InlineKeyboardButton.WithCallbackData("Unban", $"User/Unban/{selectedUser.Id}")
                    : InlineKeyboardButton.WithCallbackData("Ban", $"User/Ban/{selectedUser.Id}")
                    };

                    var sentMedia = await _bot.SendTextMessageAsync(objectBox.User.UserId,
                        $"Id: {selectedUser.UserId}\nUsername: {selectedUser.UserName}",
                        replyMarkup: new InlineKeyboardMarkup(new[] { t1, t2, t3 }));
                    TempMessage tempMessage = new TempMessage { MessageId = sentMedia.MessageId, UserId = objectBox.User.UserId };
                    await tempMessageServices.AddAsync(tempMessage);
                    await tempMessageServices.SaveAsync();
                }
                else
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, "No users found", replyMarkup: objectBox.Keyboard);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
