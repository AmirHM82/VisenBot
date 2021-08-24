using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Post.Tag
{
    public class ChosenPostsTagsCommand : ICommand
    {
        private ObjectBox objectBox;
        private int tagId;

        public ChosenPostsTagsCommand(ObjectBox objectBox, int tagId)
        {
            this.objectBox = objectBox;
            this.tagId = tagId;
        }

        public async Task Do()
        {
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var mediaService = objectBox.Provider.GetRequiredService<IMedia>();

            var tag = await tagService.FindAsync(tagId);
            var media = await mediaService.FindAsync(Guid.Parse(objectBox.User.Temp));

            tag.Medias.Add(media);
            tagService.Update(tag);
            await tagService.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
