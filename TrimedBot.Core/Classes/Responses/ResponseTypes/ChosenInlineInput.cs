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

        public override async Task Action()
        {
            List<Func<Task>> cmds = new();

            switch (user.UserLocation)
            {
                case UserLocation.Search_Posts:
                    cmds.Add(new SendSearchedPostsCommand(objectBox, Guid.Parse(chosenInlineResult.ResultId)).Do);
                    break;
                case UserLocation.Search_Users:
                    cmds.Add(new ChosenInlineSearchInUsersCommand(objectBox, long.Parse(chosenInlineResult.ResultId)).Do);
                    break;
                case UserLocation.Search_Posts_Tag:
                    cmds.Add(new ChosenPostsTagsCommand(objectBox, int.Parse(chosenInlineResult.ResultId)).Do);
                    break;
            }

            foreach (var x in cmds)
            {
                await x();
                await Task.Delay(34);
            }
        }
    }
}
