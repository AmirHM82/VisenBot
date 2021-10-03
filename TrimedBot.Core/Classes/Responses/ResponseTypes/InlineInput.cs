using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Commands.Post;
using TrimedBot.Core.Commands.User.All;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Commands.Post.Tag;
using TrimedBot.Core.Commands.User.All.BlockedTags;

namespace TrimedBot.Core.Classes.Responses.ResponseTypes
{
    public class InlineInput : Input
    {
        private ObjectBox objectBox;
        private DAL.Entities.User user;
        public InlineQuery inlineQuery;

        public InlineInput(ObjectBox objectBox, InlineQuery inlineQuery) : base(objectBox)
        {
            this.objectBox = objectBox;
            user = objectBox.User;
            this.inlineQuery = inlineQuery;
        }

        public override async Task Action(List<Func<Task>> cmds)
        {
            if (!objectBox.User.IsBanned) await ResponseInline(cmds, inlineQuery);
        }

        public async Task ResponseInline(List<Func<Task>> cmds, InlineQuery inlineQuery)
        {
            switch (user.UserState)
            {
                case UserState.Search_Users:
                    cmds.Add(new InlineSearchInUsersCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);
                    break;
                case UserState.Search_Posts_Tag:
                    cmds.Add(new SearchInPostsTagsCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);
                    break;
                case UserState.Search_User_Blocked_Tags:
                    cmds.Add(new SearchInUserBlockedTagsCommand(objectBox, inlineQuery.Id, inlineQuery.Query).Do);
                    break;
                default: /*if (user.UserLocation == UserLocation.Search_Posts)*/
                    cmds.Add(new SearchInMediasCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);
                    break;
            }
        }
    }
}
