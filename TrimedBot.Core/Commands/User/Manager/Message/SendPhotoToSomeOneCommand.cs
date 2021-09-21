using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Services;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendPhotoToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Telegram.Bot.Types.PhotoSize[] photo;
        private string caption;

        public SendPhotoToSomeOneCommand(ObjectBox objectBox, Telegram.Bot.Types.PhotoSize[] photo, string caption)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.photo = photo;
            this.caption = caption;
        }

        public async Task Do()
        {
            //await new TempMessages(objectBox).Delete();
            objectBox.IsNeedDeleteTemps = true;
            photo.SendPhoto(caption, long.Parse(objectBox.User.Temp), objectBox);            
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
