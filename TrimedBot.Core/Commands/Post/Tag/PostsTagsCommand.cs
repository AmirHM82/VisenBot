using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.Post.Tag
{
    public class PostsTagsCommand : ICommand
    {
        private ObjectBox objectBox;
        private int pageNum;

        public PostsTagsCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            if (pageNum > 0)
            {
                bool needNP = false;
                await new TempMessages(objectBox).Delete();

                List<Processor> processList = new List<Processor>();
                var tuple = await new Tags(objectBox).GetMessages(Guid.Parse(objectBox.User.Temp), pageNum);
                processList.AddRange(tuple.Item1);
                needNP = tuple.Item2;
                if (needNP)
                    processList.AddRange(new NPMessage(objectBox).CreateNP(pageNum, CallbackSection.Tag));

                new MultiProcessor(processList).AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
