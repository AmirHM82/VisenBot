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
    public class Media
    {
        public ObjectBox objectBox;

        public Media(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public void SendPrivate(DAL.Entities.Media media)
        {
            if (media != null)
            {
                new VideoResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = $"{media.Title}\n{media.Caption}",
                    Keyboard = Keyboard.PrivateMediaKeyboard(media.Id),
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No posts found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        } 

        public void SendPublic(DAL.Entities.Media media)
        {
            if (media != null)
            {
                InlineKeyboardMarkup key;
                if (!media.IsConfirmed)
                    key = Keyboard.DeclinedPublicMediaKeyboard(media.Id);
                else
                    key = Keyboard.ConfirmedPublicMediaKeyboard(media.Id);

                new VideoResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Video = media.FileId,
                    Text = $"{media.Title}\n{media.Caption}",
                    Keyboard = key,
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No posts found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }
    }
}