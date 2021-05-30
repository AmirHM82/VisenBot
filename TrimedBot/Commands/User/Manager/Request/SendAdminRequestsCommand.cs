using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedBot.Database.Sections;

namespace TrimedBot.Commands.User.Manager.Request
{
    public class SendAdminRequestsCommand : ICommand
    {
        private ObjectBox objectBox;
        protected ITempMessage tempMessageServices;
        protected IUser userServices;
        protected BotServices _bot;
        private int pageNum;

        public SendAdminRequestsCommand(IServiceProvider provider, int pageNum)
        {
            objectBox = provider.GetRequiredService<ObjectBox>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var tempSentMessagesR = new List<TempMessage>();

                var users = await userServices.GetUsersWithAdminRequestAsync(pageNum);
                if (users.Length != 0)
                {
                    if (pageNum <= 0) pageNum = 1;
                    if (users.Length == 0) pageNum = 1;

                    for (int i = 0; i < users.Length; i++)
                    {
                        var CallBackKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Accept", $"{CallbackSection.Admin}/{CallbackSection.Request}/{CallbackSection.Accept}/{users[i].UserId}"),
                            InlineKeyboardButton.WithCallbackData("Refuse", $"{CallbackSection.Admin}/{CallbackSection.Request}/{CallbackSection.Refuse}/{users[i].UserId}"),
                        });
                        var messageR = await _bot.SendTextMessageAsync(objectBox.User.UserId,
                            $"UserName: {users[i].UserName} \n Start date: {users[i].StartDate}", replyMarkup: CallBackKeyboard);
                        tempSentMessagesR.Add(new TempMessage { MessageId = messageR.MessageId, UserId = objectBox.User.UserId });
                    }

                    await tempMessageServices.AddAsync(tempSentMessagesR);
                    await tempMessageServices.SaveAsync();

                    userServices.ChangeUserPlace(objectBox.User, UserPlace.SeeAdminRequests_Manager);
                    await userServices.SaveAsync();
                }
                else await _bot.SendTextMessageAsync(objectBox.User.UserId, "No request found.");
            }
            else await _bot.SendTextMessageAsync(objectBox.User.UserId, Sentences.Access_Denied);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
