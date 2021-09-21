using System;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Post.Properties
{
    public class PostPropertiesCommand : ICommand
    {
        private ObjectBox objectBox;
        private Guid postId;

        public PostPropertiesCommand(Guid postId, ObjectBox objectBox)
        {
            this.postId = postId;
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            objectBox.IsNeedDeleteTemps = true;

            var sender = new Media(objectBox);
            if (objectBox.User.LastUserState == DAL.Enums.UserState.SeePublicAddedVideos)
                await sender.SendPublic(postId, true);
            else
                await sender.SendPrivate(postId, true);

            objectBox.User.Temp = postId.ToString();
            objectBox.UpdateUserInfo();
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
