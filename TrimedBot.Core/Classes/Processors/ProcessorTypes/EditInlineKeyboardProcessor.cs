using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class EditInlineKeyboardProcessor : Processor
    {
        public EditInlineKeyboardProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        public long ReceiverId { get; set; }
        public int MessageId { get; set; }
        public InlineKeyboardMarkup Keyboard { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.EditMessageReplyMarkupAsync(ReceiverId, MessageId, Keyboard);
        }
    }
}
