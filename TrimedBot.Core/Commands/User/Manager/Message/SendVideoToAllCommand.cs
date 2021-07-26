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
    public class SendVideoToAllCommand : ICommand
    {
        private ObjectBox objectBox;
        private Video video;
        private string caption;

        public SendVideoToAllCommand(ObjectBox objectBox, Telegram.Bot.Types.Video video, string caption)
        {
            this.objectBox = objectBox;
            this.video = video;
            this.caption = caption;
        }

        public async Task Do()
        {
            var userServices = objectBox.Provider.GetRequiredService<IUser>();
            var userIds = await userServices.GetUserIds();
            if (userIds.Length != 0)
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    video.SendVideo(caption, userIds[i], objectBox);
                }
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
