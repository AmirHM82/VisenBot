using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Post
{
    public class AddNewPostCommand : ICommand
    {
        protected BotServices _bot;
        protected ObjectBox objectBox;
        //protected IUser userServices;

        public AddNewPostCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            _bot = objectBox.Provider.GetRequiredService<BotServices>();
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public async Task Do()
        {
            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Send title of the video:",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.UserPlace = UserPlace.AddMedia_SendTitle;
            //userServices.Update(objectBox.User);
            //await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
