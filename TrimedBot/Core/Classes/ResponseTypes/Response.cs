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

        public void Callback(CallbackQuery callbackQuery)
        {
            new CallbackResponse(provider).Response(callbackQuery);
        }

        public void Inline(InlineQuery inlineQuery)
        {
            new InlineResponse(provider).Response(inlineQuery);
        }

        public void ChosenInline(ChosenInlineResult result)
        {
            new ChosenInlineResponse(provider).Response(result);
        }
    }
}
