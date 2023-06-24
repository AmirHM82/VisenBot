using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.User.All.BlockedTags
{
    public class SearchInUserBlockedTagsCommand : ICommand
    {
        private ObjectBox objectBox;
        private string queryId;
        private string name;

        public SearchInUserBlockedTagsCommand(ObjectBox objectBox, string queryId, string name)
        {
            this.objectBox = objectBox;
            this.queryId = queryId;
            this.name = name;
        }

        public async Task Do()
        {
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tags = await tagService.Search(name);
            if (objectBox.User.BlockedTags is not null)
                if (objectBox.User.BlockedTags.Count > 0)
                    tags = tags.Except(objectBox.User.BlockedTags).ToList();
            if (tags.Count != 0)
            {
                var results = new InlineQueryResultArticle[tags.Count];
                for (int i = 0; i < tags.Count && i < 50; i++)
                {
                    results[i] = new InlineQueryResultArticle(tags[i].Id.ToString(), tags[i].Name,
                        new InputTextMessageContent($"{tags[i].Id} - {tags[i].Name}"));
                }

                new InlineQueryProcessor(objectBox)
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
