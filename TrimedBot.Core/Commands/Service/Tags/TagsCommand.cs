using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class TagsCommand : ICommand
    {
        private int pageNum;
        private ObjectBox objectBox;

        public TagsCommand(int pageNum, ObjectBox objectBox)
        {
            this.pageNum = pageNum;
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            List<Processor> processList = new List<Processor>();
            if (objectBox.User.Access == DAL.Enums.Access.Manager)
            {
                if (pageNum > 0)
                {
                    bool needNP = false;
                    await new TempMessages(objectBox).Delete();

                    var tuple = await new Classes.Tags(objectBox).GetMessages(pageNum);
                    processList.AddRange(tuple.Item1);
                    needNP = tuple.Item2;
                    if (needNP)
                        processList.AddRange(new NPMessage(objectBox).CreateNP(pageNum, CallbackSection.Tag));
                }
            }
            else processList.Add(new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = objectBox.Keyboard,
                Text = Sentences.Access_Denied
            });
            new MultiProcessor(processList).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
