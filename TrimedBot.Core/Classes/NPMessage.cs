using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class NPMessage
    {
        public ObjectBox objectBox;

        public NPMessage(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public void Send(int pageNumber, string category)
        {
            if (pageNumber > 0)
            {
                var npMessage = new NPResponseProcessor()
                {
                    PageNumber = pageNumber,
                    Keyboard = Keyboard.NPKeyboard(pageNumber, category),
                    ReceiverId = objectBox.User.UserId
                };

                var textMessage = new TextResponseProcessor()
                {
                    Text = "Here you are.",
                    ReceiverId = objectBox.User.UserId,
                    Keyboard = Keyboard.CancelKeyboard(),
                    IsDeletable = true
                };

                new MultiProcessor
                    (new List<Processor>() { npMessage, textMessage })
                    .AddThisMessageToService(objectBox.Provider);
            }
        }

        public List<Processor> CreateNP(int pageNumber, string category)
        {
            if (pageNumber > 0)
            {
                var npMessage = new NPResponseProcessor()
                {
                    PageNumber = pageNumber,
                    Keyboard = Keyboard.NPKeyboard(pageNumber, category),
                    ReceiverId = objectBox.User.UserId
                };

                var textMessage = new TextResponseProcessor()
                {
                    Text = "Here you are.",
                    ReceiverId = objectBox.User.UserId,
                    Keyboard = Keyboard.CancelKeyboard(),
                    IsDeletable = true
                };

                return new List<Processor>() { npMessage, textMessage };
            }

            return null;
        }
    }
}