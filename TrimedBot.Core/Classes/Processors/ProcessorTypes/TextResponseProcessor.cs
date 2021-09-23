using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class TextResponseProcessor : Processor
    {
        //public IServiceProvider provider;
        public TextResponseProcessor(/*IServiceProvider provider*/)/* : base(provider)*/
        {
            //this.provider = provider;
        }

        public long ReceiverId { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup Keyboard { get; set; }
        public bool IsDeletable { get; set; } = false;
        public string Text { get; set; }
        public int MessageId { get; private set; }

        protected override async Task Action(IServiceProvider provider)
        {
            ITempMessage tempService = provider.GetRequiredService<ITempMessage>();
            BotServices bot = provider.GetRequiredService<BotServices>();

            var SentMessage = await bot.SendTextMessageAsync(ReceiverId, Text, ParseMode,
                replyMarkup: Keyboard);

            MessageId = SentMessage.MessageId;

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
