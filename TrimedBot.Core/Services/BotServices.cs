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
using Telegram.Bot.Extensions.Polling;
using Newtonsoft.Json;

namespace TrimedBot.Core.Services
{
    public class BotServices : TelegramBotClient, IBot
    {
        public static string Token { get; set; }
        public IServiceProvider provider;

        public BotServices(IConfiguration config, IServiceProvider provider) : base(config["TokenBetaV"]/*, new WebProxy("", )*/)
        {
            Token = config["TokenBetaV"];
            this.provider = provider;
            //OnUpdate += BotServices_OnUpdate;
        }

        //private async void BotServices_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        //{
        //    using var scope = provider.CreateScope();
        //    var scopedProvider = scope.ServiceProvider;

        //    var updateServices = provider.GetRequiredService<UpdateServices>();
        //    await updateServices.ProcessUpdate(scopedProvider, e.Update);
        //}

        public async Task StartReceiving()
        {
            var updateReceiver = new QueuedUpdateReceiver(this);
            updateReceiver.StartReceiving();
            await foreach (var update in updateReceiver.YieldUpdatesAsync())
            {
                using var scope = provider.CreateScope();
                var scopedProvider = scope.ServiceProvider;

                var updateServices = provider.GetRequiredService<UpdateServices>();
                await updateServices.ProcessUpdate(scopedProvider, update);
            }
        }
    }
}
