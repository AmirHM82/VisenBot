using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.All
{
    public class InlineSearchInUsersCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private InlineQuery query;

        public InlineSearchInUsersCommand(ObjectBox objectBox, InlineQuery query)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
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

                new InlineQueryProcessor()
                {
                    Id = query.Id,
                    Results = results
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
