using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Message
{
    public class SendPhotoToAllCommand : ICommand
    {
        private ObjectBox objectBox;
        private PhotoSize[] photo;
        protected IUser userServices;
        private string caption;

        public SendPhotoToAllCommand(ObjectBox objectBox, PhotoSize[] photo, string caption)
        {
            this.objectBox = objectBox;
            this.photo = photo;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.caption = caption;
        }

        public async Task Do()
        {
            var userIds = await userServices.GetUserIds();
            if (userIds.Length != 0)
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    photo.SendPhoto(caption, userIds[i], objectBox);
                }
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
