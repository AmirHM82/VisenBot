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
    public class AddMediaRecieveCaptionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string Caption;

        public AddMediaRecieveCaptionCommand(ObjectBox objectBox, string caption)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            Caption = caption;
        }

        public async Task Do()
        {
            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Now it's time to send the video:",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);

            objectBox.User.Temp = $"{objectBox.User.Temp}*{Caption}";
            objectBox.User.UserPlace = UserPlace.AddMedia_SendMedia;
            userServices.Update(objectBox.User);
            await userServices.SaveAsync();
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
