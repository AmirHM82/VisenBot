using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Responses;
using TrimedBot.Core.Classes.Responses.ResponseTypes;
using TrimedBot.Core.Services;
using Xunit;

namespace TrimedBot.Tests
{
    public class InlineQueryTests
    {
        [Fact]
        public void SearchFilterTest()
        {
            // Fuuuuuuuuuuuuuuuuuuuuuuuuck
            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var scope = mockScopeFactory.Object.CreateScope().ServiceProvider;
            var botService = scope.GetRequiredService<BotServices>();

            var update1 = new Update()
            {
                InlineQuery = new InlineQuery()
                {
                    Id = "1",
                    Query = "aa",
                    From = new Telegram.Bot.Types.User()
                    {
                        Id = 326683896,
                        Username = "DarknessMaster0",
                        FirstName = "AmirHM",
                        LastName = null
                    }
                }
            };
            var update2 = new Update()
            {
                InlineQuery = new InlineQuery()
                {
                    Id = "3",
                    Query = "ss",
                    From = new Telegram.Bot.Types.User()
                    {
                        Id = 1,
                        Username = "Dark",
                        FirstName = "Amir",
                        LastName = null
                    }
                }
            };

            var user = new DAL.Entities.User()
            {
                UserId = 326683896
            };

            var objectBox = new ObjectBox(scope);
            {
                objectBox.User = user;
                objectBox.Keyboard = Keyboard.StartKeyboard_Member();
                objectBox.ChatId = 326683896;
                objectBox.Settings = new DAL.Entities.Settings()
                {
                    IsResponsingAvailable = true
                };
            }

            var inlineInput = new InlineInput(objectBox, new()
            {
                ChatType = Telegram.Bot.Types.Enums.ChatType.Private,
                From = update1.InlineQuery.From,
                Id = update1.InlineQuery.Id,
                Query = update1.InlineQuery.Query
            });
            inlineInput.Response().GetAwaiter();
        }
    }
}
