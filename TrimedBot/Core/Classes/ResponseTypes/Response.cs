using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TrimedBot.Core.Classes.ResponseTypes
{
    public class Response
    {
        private IServiceProvider provider;

        public Response(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task Message(Message message)
        {
            await new MessageResponse(provider).Response(message);
        }

        public async Task Callback(CallbackQuery callbackQuery)
        {
            await new CallbackResponse(provider).Response(callbackQuery);
        }

        public async Task Inline(InlineQuery inlineQuery)
        {
            await new InlineResponse(provider).Response(inlineQuery);
        }

        public async Task ChosenInline(ChosenInlineResult result)
        {
            await new ChosenInlineResponse(provider).Response(result);
        }
    }
}
