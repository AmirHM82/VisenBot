using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class DeleteProcessor : Processor
    {
        public DeleteProcessor(ObjectBox objectBox) : base(objectBox)
        {
        }

        public long UserId { get; set; }
        public int MessageId { get; set; }

        protected override async Task Action(IServiceProvider provider)
        {
            var bot = provider.GetRequiredService<BotServices>();
            await bot.DeleteMessageAsync(UserId, MessageId);
        }
    }
}
