using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes.Responses.ResponseTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Responses
{
    public abstract class Input
    {
        private ObjectBox objectBox;

        public Input(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public bool Statue { get; set; }

        public abstract Task Action();

        public async Task Response()
        {
            try
            {
                await Action();
                Statue = true;
            }
            catch (Exception e)
            {
                e.Message.LogError();
                Statue = false;
            }
        }
    }
}
