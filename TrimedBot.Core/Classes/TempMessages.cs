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


        public async Task Delete()
        {
            var tempMessages = await tempMessageServices.GetAndDeleteAsync(objectBox.User.UserId);
            await tempMessageServices.SaveAsync();
            List<Processor> processes = new();
            if (tempMessages != null)
            {
                foreach (var item in tempMessages)
                {
                    processes.Add(new DeleteProcessor()
                    {
                        MessageId = item.MessageId,
                        UserId = item.UserId
                    });
                }
                new MultiProcessor(processes).AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task Add(params TempMessages[] temps)
        {
            return Task.CompletedTask;
        }
    }
}