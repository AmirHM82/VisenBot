using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class NPResponseProcessor : Processor
    {
        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public bool IsDeletable { get; set; } = true;
        public int PageNumber { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            ITempMessage tempService = provider.GetRequiredService<ITempMessage>();
            BotServices bot = provider.GetRequiredService<BotServices>();

            var SentMessage = await bot.SendTextMessageAsync(ReceiverId, $"Page: {PageNumber}", ParseMode,
                replyMarkup: Keyboard);

            if (IsDeletable)
            {
                await tempService.AddAsync(new TempMessage { MessageId = SentMessage.MessageId, UserId = ReceiverId });
                await tempService.SaveAsync();
            }
        }
    }
}
