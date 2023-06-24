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
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes.Channel
{
    public class ChannelVideoProcessor : Processor
    {
        public ChannelVideoProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        //public long ReceiverId { get; set; }
        public DAL.Entities.Channel Channel { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public string Text { get; set; }
        public InputOnlineFile Video { get; set; }
        public bool IsDeletable { get; set; } = false;

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            var mediaService = provider.GetRequiredService<IMedia>();

            var SentMessage = await bot.SendVideoAsync(Channel.ChatId, Video,
                caption: Text, parseMode: ParseMode, replyMarkup: Keyboard);

            //It was in TempMessages.cs. Changed to ChannelPosts.cs
            await new ChannelPosts(ObjectBox).Add(new ChannelPost
            {
                Media = await mediaService.GetAsync(Video.FileId),
                MessageId = SentMessage.MessageId,
                PostType = IsDeletable ? PostType.Temp : PostType.Regular,
                Channel = Channel
            });
        }
    }
}
