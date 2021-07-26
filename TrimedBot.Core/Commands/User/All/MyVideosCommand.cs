using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
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
            await new TempMessages(objectBox).Delete();
            await new Medias(objectBox).SendPrivate(pageNum);
            new NPMessage(objectBox).Send(pageNum, CallbackSection.Post);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
