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
    public class SendPublicMediaCommand : ICommand
    {
        private IServiceProvider provider;
        private Media media;
        protected BotServices _bot;
        protected ObjectBox objectBox;
        protected ITempMessage tempMessageServices;

        public SendPublicMediaCommand(IServiceProvider provider, Media media)
        {
            this.provider = provider;
            this.media = media;
            _bot = provider.GetRequiredService<BotServices>();
            objectBox = provider.GetRequiredService<ObjectBox>();
            tempMessageServices = provider.GetRequiredService<ITempMessage>();
        }

        public async Task Do()
        {
            if (media != null)
            {
                InlineKeyboardButton[] t1 = default;
                if (!media.IsConfirmed)
                    t1 = new InlineKeyboardButton[]
                    {
                            InlineKeyboardButton.WithCallbackData("Confirm", $"Post/Confirm/{media.Id}"),
                            InlineKeyboardButton.WithCallbackData("Delete", $"Post/Delete/{media.Id}")
                    };
                else
                    t1 = new InlineKeyboardButton[]
                    {
                            InlineKeyboardButton.WithCallbackData("Decline", $"Post/Decline/{media.Id}"),
                            InlineKeyboardButton.WithCallbackData("Delete", $"Post/Delete/{media.Id}")
                    };

                var sentMedia = await _bot.SendVideoAsync(objectBox.User.UserId, new InputOnlineFile(media.FileId),
                    caption: $"{media.Title}\n{media.Caption}", replyMarkup: new InlineKeyboardMarkup(t1));
                await tempMessageServices.AddAsync(new TempMessage { MessageId = sentMedia.MessageId, UserId = objectBox.User.UserId });
            }
            else
                await _bot.SendTextMessageAsync(objectBox.User.UserId, "No posts found", replyMarkup: objectBox.Keyboard);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
