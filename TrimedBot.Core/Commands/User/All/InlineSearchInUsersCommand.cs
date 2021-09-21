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
        private string userName;
        private string queryId;

        public InlineSearchInUsersCommand(ObjectBox objectBox, string userName, string queryId)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.queryId = queryId;
            this.userName = userName;
        }

        public async Task Do()
        {
            var seletedUsers = await userServices.Search(userName);
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
                    Id = queryId,
                    Results = results
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
