using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class SendAdminRequestsCommand : ICommand
    {
        private ObjectBox objectBox;
        private int pageNum;

        public SendAdminRequestsCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            bool needNP = false;
            List<Processor> messages = new();
            //await new TempMessages(objectBox).Delete();
            var tuple = await new AdminRequests(objectBox).CreateSendMessages(pageNum);
            messages.AddRange(tuple.Item1);
            needNP = tuple.Item2;
            if (needNP)
                messages.AddRange(new NPMessage(objectBox).CreateNP(pageNum, $"{CallbackSection.Admin}/{CallbackSection.Request}"));
            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
