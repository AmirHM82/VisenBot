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
    public class SendVideoToSomeOneCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Telegram.Bot.Types.Video video;
        private string caption;

        public SendVideoToSomeOneCommand(ObjectBox objectBox, Telegram.Bot.Types.Video video, string caption)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.video = video;
            this.caption = caption;
        }

        public Task Do()
        {
            video.SendVideo(caption, long.Parse(objectBox.User.Temp), objectBox);
            return Task.CompletedTask;
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
