using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Tags
{
    public class DeleteTagCommand : ICommand
    {
        private ObjectBox objectBox;
        private int tagId;

        public DeleteTagCommand(ObjectBox objectBox, int tagId)
        {
            this.objectBox = objectBox;
            this.tagId = tagId;
        }

        public async Task Do()
        {
            var tagsService = objectBox.Provider.GetRequiredService<ITag>();
            await tagsService.Delete(tagId);
            await tagsService.SaveAsync();

            //Edit inline keyboard
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
