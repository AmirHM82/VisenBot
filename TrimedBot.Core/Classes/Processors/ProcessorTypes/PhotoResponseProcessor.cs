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
    public class PhotoResponseProcessor : Processor
    {
        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public bool IsDeletable { get; set; } = false;
        public string Text { get; set; }
        public InputOnlineFile Photo { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.SendPhotoAsync(ReceiverId, Photo, Text, ParseMode, replyMarkup: Keyboard);

            if (IsDeletable)
            {

            }
        }
    }
}
