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
    public class EditMediaChangeCaptionCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string newCaption;

        public EditMediaChangeCaptionCommand(ObjectBox objectBox, string newCaption)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.newCaption = newCaption;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            await mediaServices.ChangeCaption(Guid.Parse(objectBox.User.Temp), newCaption);
            new TextResponseProcessor()
            {
                RecieverId = objectBox.User.UserId,
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
