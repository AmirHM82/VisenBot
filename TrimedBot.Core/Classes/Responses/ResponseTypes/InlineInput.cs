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
            List<Func<Task>> cmds = new();
            if (inlineQuery.Query != null && inlineQuery.Query != "")
                if (user.UserPlace == UserPlace.Search_Users)
                    cmds.Add(new InlineSearchInUsersCommand(objectBox, inlineQuery).Do);
                else
                    cmds.Add(new SearchInMediasCommand(objectBox, inlineQuery).Do);

            foreach (var x in cmds)
            {
                await x();
                await Task.Delay(34);
            }
        }
    }
}
