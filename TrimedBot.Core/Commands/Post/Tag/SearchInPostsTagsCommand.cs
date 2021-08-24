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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TrimedBot.Core.Commands.Post.Tag
{
    public class SearchInPostsTagsCommand : ICommand
    {
        private ObjectBox objectBox;
        private string name;
        private string queryId;

        public SearchInPostsTagsCommand(ObjectBox objectBox, string name, string queryId)
        {
            this.objectBox = objectBox;
            this.name = name;
            this.queryId = queryId;
        }

        public async Task Do()
        {
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tag = await tagService.Search(name);
            if (tag.Count != 0)
            {
                var results = new InlineQueryResultArticle[tag.Count];
                for (int i = 0; i < tag.Count && i < 50; i++)
                {
                    results[i] = new InlineQueryResultArticle(tag[i].Id.ToString(), tag[i].Name,
                        new InputTextMessageContent($"{tag[i].Id} - {tag[i].Name}"));
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
            throw new NotImplementedException();
        }
    }
}
