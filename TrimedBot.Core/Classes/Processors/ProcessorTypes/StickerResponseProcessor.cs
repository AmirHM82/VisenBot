using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class StickerResponseProcessor : Processor
    {
        //public IServiceProvider provider;
        public StickerResponseProcessor(/*IServiceProvider provider*/)/* : base(provider)*/
        {
            //this.provider = provider;
        }

        public long ReceiverId { get; set; }
        public IReplyMarkup Keyboard { get; set; }
        public InputOnlineFile Sticker { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            ITempMessage tempService = provider.GetRequiredService<ITempMessage>();
            BotServices bot = provider.GetRequiredService<BotServices>();

            await bot.SendStickerAsync(ReceiverId, Sticker, replyMarkup: Keyboard);
        }
    }
}
