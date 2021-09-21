using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class VideoResponseProcessor : Processor
    {
        //public IServiceProvider provider;
        public VideoResponseProcessor(/*IServiceProvider provider*/)/* : base(provider)*/
        {
            //this.provider = provider;
        }

        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public bool IsDeletable { get; set; } = false;
        public string Text { get; set; }
        public InputOnlineFile Video { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            BotServices bot = provider.GetRequiredService<BotServices>();
            ITempMessage tempService = provider.GetRequiredService<ITempMessage>();

            var SentMessage = await bot.SendVideoAsync(ReceiverId, Video,
                caption: Text, parseMode: ParseMode, replyMarkup: Keyboard);

            if (IsDeletable)
            {
                await tempService.AddAsync(new TempMessage
                {
                    MessageId = SentMessage.MessageId,
                    ChatId = ReceiverId,
                    Type = DAL.Enums.TempType.User
                });
                await tempService.SaveAsync();
            }
        }
    }
}
