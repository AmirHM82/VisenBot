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

namespace TrimedBot.Core.Commands.Post
{
    public class PostsCommand : ICommand
    {
        public ObjectBox objectBox;

        public PostsCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            bool needNP = false;
            List<Processor> messages = new();
            var tuple = await new Medias(objectBox).GetPublic(1);
            messages.AddRange(tuple.Item1);
            needNP = tuple.Item2;
            if (needNP)
                messages.AddRange(new NPMessage(objectBox).CreateNP(1, CallbackSection.Post));
            new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
