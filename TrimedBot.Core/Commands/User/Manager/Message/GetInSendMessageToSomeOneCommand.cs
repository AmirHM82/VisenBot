﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class GetInSendMessageToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private string userId;

        public GetInSendMessageToSomeOneCommand(ObjectBox objectBox, string userId)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.userId = userId;
        }

        public Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                objectBox.User.UserState = UserState.Send_Message_ToSomeone;
                objectBox.User.Temp = userId;
                objectBox.UpdateUserInfo();
                new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Chat started.\nSend your messages. They will be send.\n/cancel if you want to finish chatting.",
                    Keyboard = Keyboard.CancelKeyboard()
                }.AddThisMessageToService(objectBox.Provider);
            }
            else
                new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = Sentences.Access_Denied,
                    Keyboard = objectBox.Keyboard
                }.AddThisMessageToService(objectBox.Provider);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
