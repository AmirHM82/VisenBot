using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Commands.User.All
{
    public class InlineSearchInUsersCommand : ICommand
    {
        private IServiceProvider provider;
        protected IUser userServices;
        protected BotServices _bot;
        private InlineQuery query;

        public InlineSearchInUsersCommand(IServiceProvider provider, InlineQuery query)
        {
            this.provider = provider;
            userServices = provider.GetRequiredService<IUser>();
            _bot = provider.GetRequiredService<BotServices>();
            this.query = query;
        }

        public async Task Do()
        {
            var seletedUsers = await userServices.Search(query.Query);
            if (seletedUsers.Length != 0)
            {
                var results = new InlineQueryResultArticle[seletedUsers.Length];
                for (int i = 0; i < seletedUsers.Length && i < 50; i++)
                {
                    results[i] = new InlineQueryResultArticle(seletedUsers[i].UserId.ToString(), seletedUsers[i].UserName,
                        new InputTextMessageContent($"{seletedUsers[i].UserId} - {seletedUsers[i].UserName}"));
                }
                try
                {
                    await _bot.AnswerInlineQueryAsync(query.Id, results);
                }
                catch { }
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
