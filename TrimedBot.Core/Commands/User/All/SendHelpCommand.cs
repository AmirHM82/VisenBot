using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;

namespace TrimedBot.Core.Commands.User.All
{
    public class SendHelpCommand : ICommand
    {
        private ObjectBox objectBox;

        public SendHelpCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            Processor message = null;
            switch (objectBox.User.Access)
            {
                case Access.Manager:
                    message = new TextResponseProcessor()
                    {
                        RecieverId = objectBox.User.UserId,
                        Text = Sentences.Help_Manager,
                        Keyboard = objectBox.Keyboard
                    };
                    break;
                case Access.Admin:
                    message = new TextResponseProcessor()
                    {
                        RecieverId = objectBox.User.UserId,
                        Text = Sentences.Help_Admin,
                        Keyboard = objectBox.Keyboard
                    };
                    break;
                case Access.Member:
                    message = new TextResponseProcessor()
                    {
                        RecieverId = objectBox.User.UserId,
                        Text = Sentences.Help_Member,
                        Keyboard = objectBox.Keyboard
                    };
                    break;
                default:
                    break;
            }
            message?.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
