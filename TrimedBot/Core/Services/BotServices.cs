using Microsoft.CodeAnalysis.Options;
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
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Services
{
    public class BotServices : TelegramBotClient, IBot
    {
        public static string Token { get; set; }
        protected IServiceProvider Provider;
        //public static Proxy Proxy { get; set; }

        public BotServices(IConfiguration config, IServiceProvider provider) : base(config["Token"]/*, new WebProxy("", )*/)
        {
            Token = config["Token"];
            Provider = provider;
            OnUpdate += BotServices_OnUpdate;
            //OnMessage += BotServices_OnMessage;
            //OnInlineQuery += BotServices_OnInlineQuery;
            //OnCallbackQuery += BotServices_OnCallbackQuery;
            //OnInlineResultChosen += BotServices_OnInlineResultChosen;
        }

        private async void BotServices_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            var updateServices = Provider.GetRequiredService<UpdateServices>();
            await updateServices.ProcessUpdate(e.Update.Type, e.Update);
        }

        //public void SetProxy(Proxy proxy)
        //{
        //    Proxy = proxy;
        //}

        //private async void BotServices_OnInlineResultChosen(object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs e)
        //{
        //    var userServices = Provider.GetRequiredService<IUser>();
        //    var inlineServices = Provider.GetRequiredService<IInline>();
        //    var mediaServices = Provider.GetRequiredService<IMedia>();

        //    var user = await userServices.FindOrAddAsync(e.ChosenInlineResult.From.Id);
        //    await userServices.SaveAsync();

        //    if (user.UserPlace == UserPlace.Search_Posts)
        //    {
        //        var media = await mediaServices.FindAsync(Guid.Parse(e.ChosenInlineResult.ResultId));
        //        await inlineServices.SendSearchAnswer(media, user, Keyboard.CancelKeyboard);
        //    }
        //}

        //private async void BotServices_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        //{
        //    var userServices = Provider.GetRequiredService<IUser>();
        //    var messageServices = Provider.GetRequiredService<IMessage>();

        //    var user = await userServices.FindOrAddAsync(e.Message.From.Id);
        //    await userServices.SaveAsync();

        //    await messageServices.CheckAndAnswer(user, e.Message, Keyboard.SpecificKeyboard(user.Access));
        //}

        //private async void BotServices_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        //{
        //    var userServices = Provider.GetRequiredService<IUser>();
        //    var callbackServices = Provider.GetRequiredService<ICallback>();

        //    var user = await userServices.FindOrAddAsync(e.CallbackQuery.From.Id);
        //    await userServices.SaveAsync();

        //    await callbackServices.CheckAndAnswer(user, e.CallbackQuery, Keyboard.SpecificKeyboard(user.Access));
        //}

        //private async void BotServices_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        //{
        //    var userServices = Provider.GetRequiredService<IUser>();
        //    var inlineServices = Provider.GetRequiredService<IInline>();

        //    var user = await userServices.FindOrAddAsync(e.InlineQuery.From.Id);
        //    await userServices.SaveAsync();

        //    await inlineServices.CheckAndAnswer(user, e.InlineQuery, Keyboard.SpecificKeyboard(user.Access));
        //}
    }
}
