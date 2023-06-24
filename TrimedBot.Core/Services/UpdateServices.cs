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

            Input request = default;
            await objectBox.AssignSettings();

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        objectBox.ChatType = update.Message.Chat.Type;
                        await objectBox.AssignUser(update.Message.From);
                        objectBox.AssignKeyboard(objectBox.User.Access);
                        request = new MessageInput(objectBox, update.Message);
                        break;
                    case UpdateType.InlineQuery:
                        objectBox.ChatType = update.InlineQuery.ChatType;
                        await objectBox.AssignUser(update.InlineQuery.From);
                        objectBox.AssignKeyboard(objectBox.User.Access);
                        request = new InlineInput(objectBox, update.InlineQuery);
                        break;
                    case UpdateType.ChosenInlineResult:
                        //Cannot specify type for Chosen Inline :/
                        await objectBox.AssignUser(update.ChosenInlineResult.From);
                        objectBox.AssignKeyboard(objectBox.User.Access);
                        request = new ChosenInlineInput(objectBox, update.ChosenInlineResult);
                        break;
                    case UpdateType.CallbackQuery:
                        bool userValidationState = true;
                        objectBox.ChatType = update.CallbackQuery.Message.Chat.Type;
                        if (objectBox.ChatType is ChatType.Channel)
                        {
                            await objectBox.AssignChannel(update.CallbackQuery.Message.Chat);
                            userValidationState = false;
                        }
                        await objectBox.AssignUser(update.CallbackQuery.From, userValidationState);
                        objectBox.AssignKeyboard(objectBox.User.Access);
                        request = new CallbackInput(objectBox, update.CallbackQuery);
                        break;
                    case UpdateType.ChannelPost:
                        objectBox.ChatType = update.ChannelPost.Chat.Type;
                        await objectBox.AssignChannel(update.ChannelPost.Chat);
                        request = new ChannelPostInput(objectBox, update.ChannelPost);
                        break;
                }

                if (request is not null) await request.Response();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
