using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Commands.Post;
using TrimedBot.Commands.User.All;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Classes.ResponseTypes
{
    public class ChosenInlineResponse
    {
        private IServiceProvider provider { get; set; }
        private ObjectBox objectBox { get; set; }
        private Database.Models.User user { get; set; }

        public ChosenInlineResponse(IServiceProvider provider)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            user = objectBox.User;
        }

        public void Response(ChosenInlineResult result)
        {
            List<Func<Task>> cmds = new();

            switch (user.UserPlace)
            {
                case UserPlace.Search_Posts:
                    cmds.Add(new SendSearchedPostsCommand(provider, Guid.Parse(result.ResultId)).Do);
                    break;
                case UserPlace.Search_Users:
                    cmds.Add(new ChosenInlineSearchInUsersCommand(provider, result).Do);
                    break;
            }

            cmds.ForEach(async (x) => { x(); await Task.Delay(34); });
        }
    }
}
