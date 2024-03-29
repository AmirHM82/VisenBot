﻿using Microsoft.Extensions.DependencyInjection;
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
            //var cacheService = objectBox.Provider.GetRequiredService<CacheService>();

            //var videos = await cacheService.GetCachedMedias(caption);
            //if (videos?.Count() < 1)
            //{
            //    videos = await mediaServices.SearchAsync(objectBox.User, caption.ToLower());
            //    await cacheService.CacheMedias(caption, videos);
            //}
            //videos = videos.Where(x => x.IsConfirmed == true || x.User.Id == objectBox.User.Id); // It doesn't work (wtf)
            var videos = await mediaServices.SearchAsync(objectBox.User, caption.ToLower());

            if (videos?.Count() > 0)
            {
                var results = new InlineQueryResultCachedVideo[videos.Count];

                int i = 0;
                foreach (var video in videos)
                {
                    results[i] = new InlineQueryResultCachedVideo(video.Id.ToString(), video.FileId, $"{video.Title} - {video.Caption}");
                    i++;
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
