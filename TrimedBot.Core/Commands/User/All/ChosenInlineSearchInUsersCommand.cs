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
        private long userId;

        public ChosenInlineSearchInUsersCommand(ObjectBox objectBox, long userId)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.userId = userId;
        }

        public async Task Do()
        {
            var selectedUser = await userServices.FindAsync(userId);
            if (selectedUser != null)
            {
                new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = $"Id: {selectedUser.UserId}\nUsername: {selectedUser.UserName}",
                    Keyboard = Keyboard.ManageUser(selectedUser.Id, selectedUser.UserId, selectedUser.Access, selectedUser.IsBanned),
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No users found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
