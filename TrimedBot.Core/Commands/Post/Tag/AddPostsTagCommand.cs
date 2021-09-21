using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Post.Tag
{
    public class AddPostsTagCommand : ICommand
    {
        private ObjectBox objectBox;
        private Guid postId;

        public AddPostsTagCommand(ObjectBox objectBox, Guid postId)
        {
            this.objectBox = objectBox;
            this.postId = postId;
        }

        public Task Do()
        {
            objectBox.User.UserState = DAL.Enums.UserState.Search_Posts_Tag;
            objectBox.User.Temp = postId.ToString();
            objectBox.UpdateUserInfo();

            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Search name of the tag in inline mode & choose it",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
