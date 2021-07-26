using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Message
{
    public class SendPrivateMediaCommand : ICommand
    {
        private DAL.Entities.Media media;
        protected ObjectBox objectBox;

        public SendPrivateMediaCommand(ObjectBox objectBox, DAL.Entities.Media media)
        {
            this.objectBox = objectBox;
            this.media = media;
        }

        public Task Do()
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

            return Task.CompletedTask;         
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
