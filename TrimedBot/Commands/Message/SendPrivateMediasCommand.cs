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
using TrimedBot.Database.Sections;

namespace TrimedBot.Commands.Message
{
    public class SendPrivateMediasCommand : ICommand
    {
        public IServiceProvider provider { get; }
        private int pageNum;
        private ObjectBox objectBox;

        public SendPrivateMediasCommand(IServiceProvider prv, int pageNum)
        {
            provider = prv;
            this.pageNum = pageNum;
            this.objectBox = provider.GetRequiredService<ObjectBox>();
        }

        public async Task Do()
        {
            try
            {
                IMedia mediaServices = provider.GetRequiredService<IMedia>();
                ITempMessage tempMessagesServices = provider.GetRequiredService<ITempMessage>();
                IUser userServices = provider.GetRequiredService<IUser>();
                BotServices _bot = provider.GetRequiredService<BotServices>();
                var medias = await mediaServices.GetMediasAsync(objectBox.User, pageNum);
                if (medias.Length != 0)
                {
                    if (pageNum <= 0) pageNum = 1;
                    if (medias.Length == 0) pageNum = 1;

                    List<TempMessage> tempMessages = new List<TempMessage>();

                    for (int i = 0; i < medias.Length; i++)
                    {
                        InlineKeyboardButton[] t1 =
                        {
                        InlineKeyboardButton.WithCallbackData("Edit title", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Title}/{medias[i].Id}"),
                        InlineKeyboardButton.WithCallbackData("Edit caption", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Caption}/{medias[i].Id}"),
                        InlineKeyboardButton.WithCallbackData("Edit video", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Video}/{medias[i].Id}")
                    };

                        InlineKeyboardButton[] t2 =
                        {
                        InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{medias[i].Id}")
                    };

                        var media = await _bot.SendVideoAsync(objectBox.User.UserId,
                            new InputOnlineFile(medias[i].FileId), caption: $"{medias[i].Title}\n{medias[i].Caption}",
                            replyMarkup: new InlineKeyboardMarkup(new[] { t1, t2 }));
                        tempMessages.Add(new TempMessage { MessageId = media.MessageId, UserId = objectBox.User.UserId });
                    }

                    await tempMessagesServices.AddAsync(tempMessages);
                    await tempMessagesServices.SaveAsync();

                    userServices.ChangeUserPlace(objectBox.User, UserPlace.SeeAddedVideos_Member);
                    await userServices.SaveAsync();
                }
                else
                    await _bot.SendTextMessageAsync(objectBox.User.UserId, "There is no videos");
            }
            catch (Exception e)
            {
                e.Message.LogError();
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
