using System.Threading.Tasks;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes;
using System.Collections.Generic;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class AdminsCommand : ICommand
    {
        private int pageNumber;
        private ObjectBox objectBox;

        public AdminsCommand(ObjectBox objectBox, int pageNumber)
        {
            this.objectBox = objectBox;
            this.pageNumber = pageNumber;
        }

        public async Task Do()
        {
            if (pageNumber > 0)
            {
                bool needNP = false;
                List<Processor> messages = new();
                //await new TempMessages(objectBox).Delete();
                var tuple = await new Admins(objectBox).CreateSendMessages(pageNumber);
                messages.AddRange(tuple.Item1);
                needNP = tuple.Item2;
                if (needNP)
                    messages.AddRange(new NPMessage(objectBox).CreateNP(pageNumber, CallbackSection.Admin));
                new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
