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

namespace TrimedBot.Core.Commands.User.All.BlockedTags
{
    public class AddBlockedTag : ICommand
    {
        private ObjectBox objectBox;

        public AddBlockedTag(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            List<Processor> messages = new();
            var tagService = objectBox.Provider.GetRequiredService<ITag>();
            var tags = await tagService.GetTagsAsync();
            StringBuilder tagsString = new();
            foreach (var t in tags)
            {
                tagsString.Append($"{t.Name}, ");
            }
            messages.Add(new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.ChatId,
                Text = $"Existing tags: {tagsString}"
            });
            messages.Add(new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.ChatId,
                Text = "U can use inline mode to block a tag. Just simply wrtie @(bot username) and write name of the tag, then select it.",
                Keyboard = Keyboard.CancelKeyboard()
            });

            objectBox.User.UserState = DAL.Enums.UserState.Search_User_Blocked_Tags;
            objectBox.UpdateUserInfo();

            new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
