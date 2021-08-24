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

namespace TrimedBot.Core.Commands.Post.Edit
{
    public class GetInEditMediaChangeVideoSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string id;

        public GetInEditMediaChangeVideoSectionCommand(ObjectBox objectBox, string id)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.id = id;
        }

        public Task Do()
        {
            objectBox.User.UserLocation = UserLocation.EditMedia_Video;
            objectBox.User.Temp = id;
            objectBox.UpdateUserInfo();

            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Send new video:",
                Keyboard = Keyboard.CancelKeyboard()
            }.AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
