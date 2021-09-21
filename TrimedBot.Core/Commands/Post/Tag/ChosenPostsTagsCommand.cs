using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
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
            objectBox.IsNeedDeleteTemps = true;

            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var mediaService = objectBox.Provider.GetRequiredService<IMedia>();

            var tag = await tagService.FindAsync(tagId);
            var postId = Guid.Parse(objectBox.User.Temp);
            var media = await mediaService.FindAsync(postId);

            if (!media.Tags.Contains(tag))
            {
                media.Tags.Add(tag);
                mediaService.Update(media);
                await mediaService.SaveAsync();
            }

            StringBuilder tags = new StringBuilder();
            foreach (var item in media.Tags)
            {
                tags.Append($"{item.Name} ");
            }

            new VideoResponseProcessor()
            {
                IsDeletable = true,
                Keyboard = Keyboard.PublicPostProperties(postId, true),
                ReceiverId = objectBox.User.UserId,
                Text = $"{media.Title} - {media.Caption}\nTags: {tags}",
                Video = media.FileId
            }.AddThisMessageToService(objectBox.Provider);

        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
