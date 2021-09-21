using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Responses;
using TrimedBot.Core.Classes.Responses.ResponseTypes;
using TrimedBot.Core.Services;
using Xunit;

namespace TrimedBot.Tests
{
    public class MessageInputTests
    {
        [Fact]
        public async void Can_Do_StartCommand()
        {
            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var scope = mockScopeFactory.Object.CreateScope().ServiceProvider;
            var user = new DAL.Entities.User()
            {
                UserId = 326683896
            };

            var objectBox = new ObjectBox(scope);
            objectBox.User = user;
            objectBox.Keyboard = Keyboard.StartKeyboard_Member();
            objectBox.ChatId = 326683896;
            objectBox.Settings = new DAL.Entities.Settings()
            {
                IsResponsingAvailable = true
            };

            var message = new Message()
            {
                Text = "/start"
            };

            Input messageInput = new MessageInput(objectBox, message);
            await messageInput.Response();

            Assert.True(messageInput.Statue);
        }
    }
}
