using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.Service.Channels
{
    public class ChannelsCommand : ICommand
    {
        private ObjectBox objectBox;

        public ChannelsCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            List<Processor> processList = new List<Processor>();
            if (objectBox.User.Access == DAL.Enums.Access.Manager)
            {
                objectBox.IsNeedDeleteTemps = true;

                processList.AddRange(await new Classes.Channels(objectBox).GetMessages());
            }
            else processList.Add(new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = objectBox.Keyboard,
                Text = Sentences.Access_Denied
            });
            new MultiProcessor(processList, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
