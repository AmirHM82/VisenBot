using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class GetInEditTagSectionCommand : ICommand
    {
        public ObjectBox ObjectBox;
        public int TagId;

        public GetInEditTagSectionCommand(ObjectBox objectBox, int tagId)
        {
            ObjectBox = objectBox;
            TagId = tagId;
        }

        public Task Do()
        {
            throw new NotImplementedException();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
