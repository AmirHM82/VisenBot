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

namespace TrimedBot.Core.Commands.User.All.BlockedTags
{
    public class ChosenUserBlockedTags : ICommand
    {
        private ObjectBox objectBox;
        private int tagId;

        public ChosenUserBlockedTags(ObjectBox objectBox, int tagId)
        {
            this.objectBox = objectBox;
            this.tagId = tagId;
        }

        public async Task Do()
        {
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tag = await tagService.FindAsync(tagId);
            objectBox.User.BlockedTags.Add(tag);
            objectBox.UpdateUserInfo();

            new TextResponseProcessor()
            {
                ReceiverId = objectBox.ChatId,
                Text = $"{tag.Name} blocked. Use inline mode to add another tag or use cancel button (/cancel command)"
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
