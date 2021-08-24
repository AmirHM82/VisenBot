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

namespace TrimedBot.Core.Commands.User.All
{
    public class MyVideosCommand : ICommand
    {
        public ObjectBox objectBox;
        public int pageNum;

        public MyVideosCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            bool needNP = false;
            List<Processor> messages = new List<Processor>();
            await new TempMessages(objectBox).Delete();
            var tuple = await new Medias(objectBox).GetPrivate(pageNum);
            messages.AddRange(tuple.Item1);
            needNP = tuple.Item2;
            if (needNP)
                messages.AddRange(new NPMessage(objectBox).CreateNP(pageNum, CallbackSection.Post));
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
