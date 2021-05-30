using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Commands;
using TrimedBot.Commands.Message;
using TrimedBot.Commands.objectBox.User.Manager.Request;
using TrimedBot.Commands.Post;
using TrimedBot.Commands.Post.Add;
using TrimedBot.Commands.Post.Delete;
using TrimedBot.Commands.Post.Edit;
using TrimedBot.Commands.Service.Settings;
using TrimedBot.Commands.Service.Temp;
using TrimedBot.Commands.User.Admin;
using TrimedBot.Commands.User.All;
using TrimedBot.Commands.User.Manager;
using TrimedBot.Commands.User.Manager.Message;
using TrimedBot.Commands.User.Manager.Request;
using TrimedBot.Commands.User.Manager.Settings;
using TrimedBot.Commands.User.Member;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

namespace TrimedBot.Core.Classes.ResponseTypes
{
    public class InlineResponse
    {
        private IServiceProvider provider { get; set; }
        private ObjectBox objectBox { get; set; }
        private Database.Models.User user { get; set; }

        public InlineResponse(IServiceProvider provider)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            user = objectBox.User;
        }

        public async Task Response(InlineQuery inlineQuery)
        {
            List<Func<Task>> cmds = new();
            if (inlineQuery.Query != null && inlineQuery.Query != "")
                if (user.UserPlace == UserPlace.Search_Users)
                    cmds.Add(new InlineSearchInUsersCommand(provider, inlineQuery).Do);
                else
                    cmds.Add(new SearchInMediasCommand(provider, inlineQuery).Do);

            foreach (var x in cmds)
            {
                await x();
                await Task.Delay(34);
            }
        }
    }
}
