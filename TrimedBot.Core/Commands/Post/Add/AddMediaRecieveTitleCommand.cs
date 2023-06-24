using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post.Add
{
    public class AddMediaRecieveTitleCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string title;

        public AddMediaRecieveTitleCommand(ObjectBox objectBox, string title)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.title = title;
        }

        public Task Do()
        {
            new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Please send caption of the video:",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.Temp = title;
            objectBox.User.UserState = UserState.AddMedia_SendCaption;
            objectBox.UpdateUserInfo();
            //userServices.Update(objectBox.User);
            //await userServices.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
