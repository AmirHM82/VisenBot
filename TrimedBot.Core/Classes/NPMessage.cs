using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
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
                new NPResponseProcessor(objectBox)
                {
                    PageNumber = pageNumber,
                    Keyboard = Keyboard.NPKeyboard(pageNumber, category),
                    ReceiverId = objectBox.User.UserId
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public List<Processor> CreateNP(int pageNumber, string category)
        {
            if (pageNumber > 0)
            {
                var npMessage = new NPResponseProcessor(objectBox)
                {
                    PageNumber = pageNumber,
                    Keyboard = Keyboard.NPKeyboard(pageNumber, category),
                    ReceiverId = objectBox.User.UserId
                };

                return new List<Processor>() { npMessage };
            }

            return null;
        }
    }
}