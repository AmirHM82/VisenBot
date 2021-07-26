using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Message
{
    public class SendNPMessageCommand : ICommand
    {
        public int pageNumber;
        public string Category;
        private ObjectBox objectBox;

        public SendNPMessageCommand(ObjectBox objectBox, int pageNumber, string category)
        {
            this.pageNumber = pageNumber;
            Category = category;
            this.objectBox = objectBox;
        }

        public Task Do()
        {
            if (objectBox.User.UserPlace != DAL.Enums.UserPlace.NoWhere)
            {
                if (pageNumber > 0)
                {
                    var npMessage = new NPResponseProcessor()
                    {
                        PageNumber = pageNumber,
                        Keyboard = Keyboard.NPKeyboard(pageNumber, Category),
                        ReceiverId = objectBox.User.UserId
                    };

                    var textMessage = new TextResponseProcessor()
                    {
                        Text = "Here you are.",
                        ReceiverId = objectBox.User.UserId,
                        Keyboard = Keyboard.CancelKeyboard()
                    };

                    new MultiProcessor
                        (new List<Classes.Processors.Processor>() { npMessage, textMessage })
                        .AddThisMessageToService(objectBox.Provider);
                }
            }
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
