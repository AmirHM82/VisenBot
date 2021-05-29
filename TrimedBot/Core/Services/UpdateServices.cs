using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Commands;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.ResponseTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Services
{
    public class UpdateServices
    {
        private IServiceProvider _provider;

        public UpdateServices(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ProcessUpdate(UpdateType type, Update update)
        {
            using var scope = _provider.CreateScope();
            _provider = scope.ServiceProvider;
            var objectBox = _provider.GetRequiredService<ObjectBox>();
            Response response = new Response(_provider);

            switch (type)
            {
                case UpdateType.Message:
                    if (update.Message.Chat.Type == ChatType.Private &&
                        (update.Message.Type == MessageType.Text || update.Message.Type == MessageType.Video))
                    {
                        await objectBox.AssignUser(update.Message.From);
                        objectBox.AssignKeyboard(objectBox.User.Access);
                        await objectBox.AssignSettings();
                        response.Message(update.Message);
                    }
                    break;
                case UpdateType.InlineQuery:
                    await objectBox.AssignUser(update.InlineQuery.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    await objectBox.AssignSettings();
                    response.Inline(update.InlineQuery);
                    break;
                case UpdateType.ChosenInlineResult:
                    await objectBox.AssignUser(update.ChosenInlineResult.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    await objectBox.AssignSettings();
                    response.ChosenInline(update.ChosenInlineResult);
                    break;
                case UpdateType.CallbackQuery:
                    await objectBox.AssignUser(update.CallbackQuery.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    await objectBox.AssignSettings();
                    response.Callback(update.CallbackQuery);
                    break;
                default:
                    return;
            }
        }
    }
}
