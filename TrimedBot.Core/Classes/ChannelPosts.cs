using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class ChannelPosts
    {
        private ObjectBox objectBox;
        private IChannelPost channelPostService;

        public ChannelPosts(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            channelPostService = objectBox.Provider.GetRequiredService<IChannelPost>();
        }

        public async Task Add(ChannelPost channelPost)
        {
            await channelPostService.AddAsync(channelPost);
            await channelPostService.SaveAsync();
        }

        public async Task Delete(ChannelPost channelPost)
        {
            channelPostService.Delete(channelPost);
            await channelPostService.SaveAsync();
        }

        public async Task<List<Processor>> Delete(Guid mediaId)
        {
            List<Processor> messages = new();
            var posts = await channelPostService.GetAndDelete(mediaId);

            if (posts != null)
                foreach (var p in posts)
                {
                    if (p.Channel is not null)
                    messages.Add(new DeleteProcessor(objectBox)
                    {
                        UserId = p.Channel.ChatId,
                        MessageId = p.MessageId
                    });
                }

            await channelPostService.SaveAsync();
            return messages;
        }

        public async Task DeleteTemps(long chatId)
        {
            var channelPosts = await channelPostService.GetAndDelete(chatId, PostType.Temp);
            List<Processor> processes = new();
            if (channelPosts != null)
            {
                foreach (var post in channelPosts)
                {
                    processes.Add(new DeleteProcessor(objectBox)
                    {
                        MessageId = post.MessageId,
                        UserId = chatId
                    });
                }

                new MultiProcessor(processes, objectBox).AddThisMessageToService(objectBox.Provider);
            }
        }
    }
}
