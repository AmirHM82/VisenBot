using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class InlineQueryProcessor : Processor
    {
        public InlineQueryProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        public string Id { get; set; }
        public IEnumerable<InlineQueryResultBase> Results { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.AnswerInlineQueryAsync(Id, Results);
        }
    }
}
