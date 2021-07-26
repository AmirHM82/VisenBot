using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.Post.Edit
{
    public class EditMediaChangeTitleCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string newTitle;

        public EditMediaChangeTitleCommand(ObjectBox objectBox, string newTitle)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.newTitle = newTitle;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            await mediaServices.ChangeTitle(Guid.Parse(objectBox.User.Temp), newTitle);

            new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Edited",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);

            await userServices.Reset(objectBox.User, new UserResetSection[] { UserResetSection.Temp, UserResetSection.UserPlace });
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
