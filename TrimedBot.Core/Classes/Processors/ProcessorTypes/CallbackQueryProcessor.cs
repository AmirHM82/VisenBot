using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class CallbackQueryProcessor : Processor
    {
        public CallbackQueryProcessor()
        {
        }

        public string Id { get; set; }
        public string Text { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.AnswerCallbackQueryAsync(Id, Text);
        }
    }
}
