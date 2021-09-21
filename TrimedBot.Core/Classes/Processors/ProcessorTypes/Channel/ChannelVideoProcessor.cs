using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes.Channel
{
    public class ChannelVideoProcessor : Processor
    {
        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public string Text { get; set; }
        public InputOnlineFile Video { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            var channelService = provider.GetRequiredService<IChannel>();
            var mediaService = provider.GetRequiredService<IMedia>();

            var SentMessage = await bot.SendVideoAsync(ReceiverId, Video,
                caption: Text, parseMode: ParseMode, replyMarkup: Keyboard);

            await channelService.AddPostAsync(new DAL.Entities.ChannelPost
            {
                Media = await mediaService.GetAsync(Video.FileId),
                MessageId = SentMessage.MessageId
            });
            await channelService.SaveAsync();
        }
    }
}
