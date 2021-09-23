using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.User.All.BlockedTags
{
    public class PostsFiltersCommand : ICommand
    {
        private ObjectBox objectBox;

        public PostsFiltersCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            bool needNP = false;
            List<Processor> messages = new();

            var tupleResult = await new Classes.Tags(objectBox).GetMessages(objectBox.User.BlockedTags.ToList());
            messages.AddRange(tupleResult.Item1);
            needNP = tupleResult.Item2;
            if (needNP)
                messages.AddRange(new NPMessage(objectBox).CreateNP(1, $"{CallbackSection.User}/{CallbackSection.Tag}"));
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
