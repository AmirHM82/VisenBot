using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ChosenInlineInput : Input
    {
        private ObjectBox objectBox;
        private DAL.Entities.User user;
        public ChosenInlineResult chosenInlineResult;

        public ChosenInlineInput(ObjectBox objectBox, ChosenInlineResult chosenInlineResult) : base(objectBox)
        {
            this.objectBox = objectBox;
            user = objectBox.User;
            this.chosenInlineResult = chosenInlineResult;
        }

        public override async Task Action(List<Func<Task>> cmds)
        {
            if (!objectBox.User.IsBanned) await ResponseChosenInline(cmds, chosenInlineResult);
        }

        public async Task ResponseChosenInline(List<Func<Task>> cmds, ChosenInlineResult chosenInlineResult)
        {
            switch (user.UserState)
            {
                case UserState.Search_Posts:
                    cmds.Add(new SendSearchedPostsCommand(objectBox, Guid.Parse(chosenInlineResult.ResultId)).Do);
                    break;
                case UserState.Search_Users:
                    cmds.Add(new ChosenInlineSearchInUsersCommand(objectBox, long.Parse(chosenInlineResult.ResultId)).Do);
                    break;
                case UserState.Search_Posts_Tag:
                    cmds.Add(new ChosenPostsTagsCommand(objectBox, int.Parse(chosenInlineResult.ResultId)).Do);
                    break;
                case UserState.Search_User_Blocked_Tags:
                    cmds.Add(new ChosenUserBlockedTags(objectBox, int.Parse(chosenInlineResult.ResultId)).Do);
                    break;
            }
        }
    }
}
