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

        public TempMessages(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
        }

        public async Task Delete(long chatId)
        {
            var tempMessages = await tempMessageServices.GetAndDeleteAsync(chatId);
            await tempMessageServices.SaveAsync();
            List<Processor> processes = new();
            if (tempMessages != null)
            {
                foreach (var item in tempMessages)
                {
                    processes.Add(new DeleteProcessor()
                    {
                        MessageId = item.MessageId,
                        UserId = item.ChatId
                    });
                }
                new MultiProcessor(processes).AddThisMessageToService(objectBox.Provider);
            }
        }

        public async Task Add(List<TempMessage> temps)
        {
            await tempMessageServices.AddAsync(temps);
            await tempMessageServices.SaveAsync();
        }

        public async Task Add(TempMessage temp)
        {
            await tempMessageServices.AddAsync(temp);
            await tempMessageServices.SaveAsync();
        }
    }

    public static class StaticTempMessages
    {
        public static async Task DeleteTemps(this ObjectBox objectBox, long chatId)
        {
            if (objectBox.IsNeedDeleteTemps)
            {
                await new TempMessages(objectBox).Delete(chatId);
            }
        }
    }
}