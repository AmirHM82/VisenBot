using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Commands.Post
{
    public class SearchInMediasCommand : ICommand
    {
        private IServiceProvider provider;
        protected IMedia mediaServices;
        private ObjectBox objectBox;
        private BotServices _bot;
        private InlineQuery query;

        public SearchInMediasCommand(IServiceProvider provider, InlineQuery query)
        {
            this.provider = provider;
            var mediaServices = provider.GetRequiredService<IMedia>();
            this.query = query;
        }

        public async Task Do()
        {
            var videos = await mediaServices.SearchAsync(objectBox.User.Id, query.Query.ToLower());

            if (videos != null)
            {
                var results = new InlineQueryResultCachedVideo[videos.Length];

                for (int i = 0; i < videos.Length && i < 50; i++)
                {
                    results[i] = new InlineQueryResultCachedVideo(videos[i].Id.ToString(), videos[i].FileId, $"{videos[i].Title} - {videos[i].Caption}");
                }
                await _bot.AnswerInlineQueryAsync(query.Id, results);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
