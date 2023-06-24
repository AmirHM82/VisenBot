using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TrimedBot.Core.Interfaces;
using Telegram.Bot.Extensions.Polling;
using Dasync.Collections;

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

            await updateReceiver.YieldUpdatesAsync().ParallelForEachAsync(Handle, 1000);

            //await foreach (var update in updateReceiver.YieldUpdatesAsync()) Handle(update);
        }

        public async Task Handle(Update update)
        {
            
            

            var updateServices = provider.GetRequiredService<UpdateServices>();
            await updateServices.ProcessUpdate(provider, update);
        }
    }
}
