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
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Post
{
    public class SearchInMediasCommand : ICommand
    {
        private ObjectBox objectBox;
        private string caption;
        private string queryId;

        public SearchInMediasCommand(ObjectBox objectBox, string caption, string queryId)
        {
            this.objectBox = objectBox;
            this.caption = caption;
            this.queryId = queryId;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var videos = await mediaServices.SearchAsync(objectBox.User, caption.ToLower());

            if (videos != null)
            {
                var results = new InlineQueryResultCachedVideo[videos.Length];

                for (int i = 0; i < videos.Length && i < 50; i++)
                {
                    results[i] = new InlineQueryResultCachedVideo(videos[i].Id.ToString(), videos[i].FileId, $"{videos[i].Title} - {videos[i].Caption}");
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
