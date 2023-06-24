using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post
{
    public class GetInSearchInPostsSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        //private IUser userServices;

        public GetInSearchInPostsSectionCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public Task Do()
        {
            new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "You can find posts here with inline mode.",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserState = UserState.Search_Posts;
            objectBox.UpdateUserInfo();
            //userServices.ChangeUserPlace(objectBox.User, UserPlace.Search_Posts);
            //await userServices.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
