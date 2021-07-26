using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.All
{
    public class ChosenInlineSearchInUsersCommand : ICommand
    {
        protected IUser userServices;
        protected ObjectBox objectBox;
        private ChosenInlineResult result;

        public ChosenInlineSearchInUsersCommand(ObjectBox objectBox, ChosenInlineResult result)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.result = result;
        }

        public async Task Do()
        {
            var selectedUser = await userServices.FindAsync(long.Parse(result.ResultId));
            if (selectedUser != null)
            {
                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = $"Id: {selectedUser.UserId}\nUsername: {selectedUser.UserName}",
                    Keyboard = Keyboard.ManageUser(selectedUser.Id, selectedUser.UserId, selectedUser.Access, selectedUser.IsBanned),
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No users found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
