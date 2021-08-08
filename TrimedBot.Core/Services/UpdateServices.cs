using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes.Responses;
using TrimedBot.Core.Classes.Responses.ResponseTypes;

namespace TrimedBot.Core.Services
{
    public class UpdateServices
    {
        public UpdateServices()
        {

        }

        public async Task ProcessUpdate(IServiceProvider provider, Update update)
        {
            var objectBox = provider.GetRequiredService<ObjectBox>();

            Input request;
            await objectBox.AssignSettings();

            switch (update.Type)
            {
                case UpdateType.Message:
                    //if (update.Message.Chat.Type == ChatType.Private &&
                    //    (update.Message.Type == MessageType.Text || update.Message.Type == MessageType.Video || update.Message.Type == MessageType.Photo))
                    //{
                    //}
                    await objectBox.AssignUser(update.Message.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    request = new MessageInput(objectBox, update.Message);
                    await request.Response();
                    break;
                case UpdateType.InlineQuery:
                    await objectBox.AssignUser(update.InlineQuery.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    request = new InlineInput(objectBox, update.InlineQuery);
                    await request.Response();
                    break;
                case UpdateType.ChosenInlineResult:
                    await objectBox.AssignUser(update.ChosenInlineResult.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    request = new ChosenInlineInput(objectBox, update.ChosenInlineResult);
                    await request.Response();
                    break;
                case UpdateType.CallbackQuery:
                    await objectBox.AssignUser(update.CallbackQuery.From);
                    objectBox.AssignKeyboard(objectBox.User.Access);
                    request = new CallbackInput(objectBox, update.CallbackQuery);
                    await request.Response();
                    break;
                default:
                    return;
            }
        }
    }
}
