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

        public async Task Do()
        {
            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "You can find posts here with inline mode.",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserPlace = UserPlace.Search_Posts;
            //userServices.ChangeUserPlace(objectBox.User, UserPlace.Search_Posts);
            //await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
