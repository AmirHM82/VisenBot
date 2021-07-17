using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Commands.Service.Temp
{
    public class DeleteTempMessagesCommand : ICommand
    {
        protected IUser userServices;
        private ObjectBox objectBox;
        private ITempMessage tempMessageServices;

        public DeleteTempMessagesCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
        }

        public async Task Do()
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

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
