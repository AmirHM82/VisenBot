using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class VoiceResponseProcessor : Processor
    {
        public VoiceResponseProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        public long ReceiverId { get; set; }
        public InputOnlineFile Voice { get; set; }
        public string Text { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.SendVoiceAsync(ReceiverId, Voice, Text, ParseMode, replyMarkup: Keyboard);
        }
    }
}
