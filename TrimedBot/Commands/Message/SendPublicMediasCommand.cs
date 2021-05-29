using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Commands.Message
{
    public class SendPublicMediasCommand : ICommand
    {
        private IServiceProvider provider;
        protected IMedia mediaServices;
        protected ITempMessage tempMessageServices;
        protected IUser userServices;
        protected BotServices _bot;
        private ObjectBox objectBox;
        private int pageNum;

        public SendPublicMediasCommand(IServiceProvider provider, int pageNum)
        {
            this.provider = provider;
            mediaServices = provider.GetRequiredService<IMedia>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
            {
                var medias = await mediaServices.GetNotConfirmedPostsAsync(pageNum);
                if (medias.Length != 0)
                {
                    if (pageNum <= 0) pageNum = 1;
                    if (medias.Length == 0) pageNum = 1;

                    List<TempMessage> tempMessages = new List<TempMessage>();

                    for (int i = 0; i < medias.Length; i++)
                    {
                        InlineKeyboardButton[] t1 =
                        {
                            InlineKeyboardButton.WithCallbackData("Confirm", $"Post/Confirm/{medias[i].Id}"),
                            InlineKeyboardButton.WithCallbackData("Delete", $"Post/Delete/{medias[i].Id}")
                        };

                        var media = await _bot.SendVideoAsync(objectBox.User.UserId,
                            new InputOnlineFile(medias[i].FileId), caption: $"{medias[i].Title}\n{medias[i].Caption}",
                            replyMarkup: new InlineKeyboardMarkup(t1));
                        tempMessages.Add(new TempMessage { MessageId = media.MessageId, UserId = objectBox.User.UserId });
                    }
                    await tempMessageServices.AddAsync(tempMessages);
                    await tempMessageServices.SaveAsync();

                    userServices.ChangeUserPlace(objectBox.User, objectBox.User.Access == Access.Admin
                       ? UserPlace.SeeAddedVideos_Admin
                       : UserPlace.SeeAddedVideos_Manager);
                    await userServices.SaveAsync();
                }
                else await _bot.SendTextMessageAsync(objectBox.User.UserId, "No medias found.");
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
