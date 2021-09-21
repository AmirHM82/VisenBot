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
using Telegram.Bot.Types;
using TrimedBot.Core.Classes.Processors;

namespace TrimedBot.Core.Commands.Post.Edit
{
    public class GetInEditMediaChangeCaptionSectionCommand : ICommand
    {
        private ObjectBox objectBox;
        private string id;
        private int messageId;

        public GetInEditMediaChangeCaptionSectionCommand(ObjectBox objectBox, string id, int messageId)
        {
            this.objectBox = objectBox;
            this.id = id;
            this.messageId = messageId;
        }

        public Task Do()
        {
            objectBox.User.UserState = UserState.EditMedia_Caption;
            objectBox.User.Temp = id;
            objectBox.UpdateUserInfo();

            List<Processor> processes = new();

            processes.Add(new DeleteInlineKeyboardProcessor()
            {
                MessageId = messageId,
                ReceiverId = objectBox.User.UserId
            });

            processes.Add(new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "Send new caption:",
                Keyboard = Keyboard.CancelKeyboard()
            });

            new MultiProcessor(processes).AddThisMessageToService(objectBox.Provider);

            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
