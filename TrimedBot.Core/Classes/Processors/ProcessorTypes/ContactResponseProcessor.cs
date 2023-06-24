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
    public class ContactResponseProcessor : Processor
    {
        public ContactResponseProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        public long ReceiverId { get; set; }
        public IReplyMarkup Keyboard { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            BotServices bot = provider.GetRequiredService<BotServices>();

            await bot.SendContactAsync(ReceiverId, PhoneNumber, FirstName, LastName, replyMarkup: Keyboard);
        }
    }
}
