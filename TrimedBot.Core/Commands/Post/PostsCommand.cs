using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
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
            await new Medias(objectBox).SendPublic(1);
            new NPMessage(objectBox).Send(1, CallbackSection.Post);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
