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

        public override async Task Action()
        {
            if (!objectBox.User.IsBanned) await ResponseInline(inlineQuery);
        }

        public async Task ResponseInline(InlineQuery inlineQuery)
        {
            List<Func<Task>> cmds = new();
            if (inlineQuery.Query != null && inlineQuery.Query != "")
                if (user.UserState == UserState.Search_Users)
                    cmds.Add(new InlineSearchInUsersCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);
                else if (user.UserState == UserState.Search_Posts_Tag)
                    cmds.Add(new SearchInPostsTagsCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);
                else /*if (user.UserLocation == UserLocation.Search_Posts)*/
                    cmds.Add(new SearchInMediasCommand(objectBox, inlineQuery.Query, inlineQuery.Id).Do);

            foreach (var x in cmds)
            {
                await x();
                await Task.Delay(34);
            }
        }
    }
}
