using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class ForwardProcessor : Processor
    {
        public long RecieverId { get; set; }
        public long FromId { get; set; }
        public int messageId { get; set; }
        public bool IsDeletable { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.ForwardMessageAsync(RecieverId, FromId, messageId);

            if (IsDeletable)
            {

            }
        }
    }
}
