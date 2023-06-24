using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.User.All.BlockedTags
{
    public class BlockedTagsCommand : ICommand
    {
        private ObjectBox objectBox;
        private int pageNum;

        public BlockedTagsCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public Task Do()
        {
            bool needNP = false;
            List<Processor> messages = new();
            var tags = objectBox.User.BlockedTags is not null ? objectBox.User.BlockedTags.ToList() : null;
            var tupleResult = new Tags(objectBox).GetMessages(tags);
            messages.AddRange(tupleResult.Item1);
            needNP = tupleResult.Item2;
            if (needNP)
                messages.AddRange(new NPMessage(objectBox).CreateNP(pageNum, $"{CallbackSection.User}/{CallbackSection.Tag}"));
            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
