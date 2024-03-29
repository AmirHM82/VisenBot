using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class TempMessages
    {
        public ObjectBox objectBox;
        public ITempMessage tempMessageServices;
        public IChannelPost channelPostServices;

        public TempMessages(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            channelPostServices = objectBox.Provider.GetRequiredService<IChannelPost>();
        }

        public async Task DeleteUserTemps(long chatId)
        {
            var tempMessages = await tempMessageServices.GetAndDeleteAsync(chatId);
            await tempMessageServices.SaveAsync();
            List<Processor> processes = new();
            if (tempMessages != null)
            {
                foreach (var item in tempMessages)
                {
                    processes.Add(new DeleteProcessor(objectBox)
                    {
                        MessageId = item.MessageId,
                        UserId = chatId
                    });
                }
                new MultiProcessor(processes, objectBox).AddThisMessageToService(objectBox.Provider);
            }
        }

        public async Task AddUserTemps(List<TempMessage> temps)
        {
            await tempMessageServices.AddAsync(temps);
            await tempMessageServices.SaveAsync();
        }

        public async Task AddUserTemp(TempMessage temp)
        {
            await tempMessageServices.AddAsync(temp);
            await tempMessageServices.SaveAsync();
        }

        //It wasn't a good idea. We have PostType prop in ChannelPost.cs model
        //So I just moved them in ChannelPosts.cs with some changes
        //public async Task AddChannelTemps(List<ChannelPost> posts) 
        //{
        //    await channelPostServices.AddAsync(posts);
        //    await channelPostServices.SaveAsync();
        //}

        //public async Task AddChannelTemp(ChannelPost post)
        //{
        //    await channelPostServices.AddAsync(post);
        //    await channelPostServices.SaveAsync();
        //}
    }

    public static class StaticTempMessages
    {
        public static async Task DeleteTemps(this ObjectBox objectBox, long chatId)
        {
            if (objectBox.IsNeedDeleteTemps)
            {
                if (objectBox.ChatType.HasValue)
                    if (objectBox.ChatType == ChatType.Channel)
                        await new ChannelPosts(objectBox).DeleteTemps(chatId);
                    else
                        await new TempMessages(objectBox).DeleteUserTemps(chatId);
            }
        }
    }
}