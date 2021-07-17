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

namespace TrimedBot.Core.Commands.User.Manager.Settings
{
    public class OpenSettingsMenuCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;

        public OpenSettingsMenuCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                new TextResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    Text = "Settings menu:",
                    Keyboard = Keyboard.SettingsKeyboard()
                }.AddThisMessageToService(objectBox.Provider);

                userServices.ChangeUserPlace(objectBox.User, UserPlace.Settings_Menu);
                await userServices.SaveAsync();
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
