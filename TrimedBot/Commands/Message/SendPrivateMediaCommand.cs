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
    public class SendPrivateMediaCommand : ICommand
    {
        private IServiceProvider provider;
        private Media media;
        protected BotServices _bot;
        protected ObjectBox objectBox;
        protected ITempMessage tempMessageServices;

        public SendPrivateMediaCommand(IServiceProvider provider, Media media)
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
                InlineKeyboardButton[] t1 =
                {
                        InlineKeyboardButton.WithCallbackData("Edit title", $"Post/Edit/Title/{media.Id}"),
                        InlineKeyboardButton.WithCallbackData("Edit caption", $"Post/Edit/Caption/{media.Id}"),
                        InlineKeyboardButton.WithCallbackData("Edit video", $"Post/Edit/Video/{media.Id}")
                    };

                InlineKeyboardButton[] t2 =
                {
                        InlineKeyboardButton.WithCallbackData("Delete", $"Post/Delete/{media.Id}")
                    };

                var sentMedia = await _bot.SendVideoAsync(objectBox.User.UserId, new InputOnlineFile(media.FileId),
                    caption: $"{media.Title}\n{media.Caption}", replyMarkup: new InlineKeyboardMarkup(new[] { t1, t2 }));
                await tempMessageServices.AddAsync(new TempMessage { MessageId = sentMedia.MessageId, UserId = objectBox.User.UserId });
                await tempMessageServices.SaveAsync();
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
