using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class BotServices : TelegramBotClient, IBot
    {
        public static string Token { get; set; }
        protected IServiceProvider provider;

        public BotServices(IConfiguration config, IServiceProvider provider) : base(config["Token"]/*, new WebProxy("", )*/)
        {
            Token = config["Token"];
            this.provider = provider;
            OnUpdate += BotServices_OnUpdate;
        }

        private async void BotServices_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            using var scope = provider.CreateScope();
            provider = scope.ServiceProvider;

            var updateServices = provider.GetRequiredService<UpdateServices>();
            await updateServices.ProcessUpdate(provider, e.Update);
        }
    }
}
