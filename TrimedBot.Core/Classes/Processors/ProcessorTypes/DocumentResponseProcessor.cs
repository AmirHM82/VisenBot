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
using Telegram.Bot.Types;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class DocumentResponseProcessor : Processor
    {
        //public IServiceProvider provider;
        public DocumentResponseProcessor(/*IServiceProvider provider*/)/* : base(provider)*/
        {
            //this.provider = provider;
        }

        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public string Text { get; set; }
        public InputOnlineFile Document { get; set; }
        public InputMedia Thumb { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            BotServices bot = provider.GetRequiredService<BotServices>();
            ITempMessage tempService = provider.GetRequiredService<ITempMessage>();

            await bot.SendDocumentAsync(ReceiverId, Document, Thumb, Text, ParseMode, replyMarkup: Keyboard);
        }
    }
}
