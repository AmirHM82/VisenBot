using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedBot.Database.Sections;

namespace TrimedBot.Commands.User.Manager.Request
{
    public class SendAdminsCommand : ICommand
    {
        private int pageNumber;
        private IServiceProvider provider;
        protected BotServices _bot;
        protected ITempMessage tempMessageServices;
        protected IUser userServices;
        private ObjectBox objectBox;

        public SendAdminsCommand(IServiceProvider provider, int pageNumber)
        {
            this.pageNumber = pageNumber;
            this.provider = provider;
            _bot = provider.GetRequiredService<BotServices>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            userServices = provider.GetRequiredService<IUser>();
            objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var tempMessages = new List<TempMessage>();
                Database.Models.User[] admins = await userServices.GetAdminsAsync(pageNumber);
                if (admins.Length > 0)
                {
                    if (pageNumber <= 0) pageNumber = 1;
                    if (admins.Length == 0) pageNumber = 1;

                    for (int i = 0; i < admins.Length; i++)
                    {
                        InlineKeyboardButton[] p1 =
                        {
                                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Admin}/{CallbackSection.Delete}/{admins[i].Id}")
                                };

                        var adminMessage = await _bot.SendTextMessageAsync(objectBox.User.UserId,
                            $"UserName: {admins[i].UserName} \n Start date: {admins[i].StartDate}",
                            replyMarkup: new InlineKeyboardMarkup(p1));
                        tempMessages.Add(new TempMessage { MessageId = adminMessage.MessageId, UserId = objectBox.User.UserId });
                    }
                    await tempMessageServices.AddAsync(tempMessages);
                    await tempMessageServices.SaveAsync();

                    userServices.ChangeUserPlace(objectBox.User, UserPlace.SeeAdmins_Manager);
                    await userServices.SaveAsync();
                }
                else
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, "Admins not found");
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
