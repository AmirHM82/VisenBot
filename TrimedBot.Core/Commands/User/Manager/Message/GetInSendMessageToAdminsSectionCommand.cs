using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class GetInSendMessageToAdminsSectionCommand : ICommand
    {
        private ObjectBox objectBox;

        public GetInSendMessageToAdminsSectionCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                objectBox.User.UserState = UserState.Send_Message_ToAdmins;
                objectBox.UpdateUserInfo();

                new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Send your messages:",
                    Keyboard = Keyboard.CancelKeyboard()
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor(objectBox)
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
