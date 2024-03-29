﻿using Microsoft.Extensions.DependencyInjection;
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

        public OpenSettingsMenuCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Settings menu:",
                    Keyboard = Keyboard.SettingsKeyboard()
                }.AddThisMessageToService(objectBox.Provider);

                //objectBox.User.UserState = UserState.Settings_Menu; //We need some changes in settings
                //objectBox.UpdateUserInfo();
            }
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
