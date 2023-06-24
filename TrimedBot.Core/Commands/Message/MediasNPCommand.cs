using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Commands.Message
{
    public class MediasNPCommand : ICommand
    {
        public ObjectBox objectBox;
        public int pageNum;
        public string Category;

        public MediasNPCommand(ObjectBox objectBox, int pageNum, string category)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
            Category = category;
        }

        public async Task Do()
        {
            if (pageNum > 0)
            {
                bool needNP = false;
                //await new TempMessages(objectBox).Delete();
                objectBox.IsNeedDeleteTemps = true;

                List<Processor> messages = new();
                Tuple<List<Processor>, bool> tuple = null;
                //if (objectBox.User.UserLocation == UserLocation.SeeAddedVideos_Member)
                if (objectBox.User.Temp == "SendPrivateMedias")
                    tuple = await new Medias(objectBox).GetPrivate(pageNum);
                //else if (objectBox.User.UserLocation == UserLocation.SeeAddedVideos_Admin || 
                //    objectBox.User.UserLocation == UserLocation.SeeAddedVideos_Manager)
                else if (objectBox.User.Temp == "SendPublicMedias")
                    tuple = await new Medias(objectBox).GetPublic(pageNum);

                messages.AddRange(tuple.Item1);
                needNP = tuple.Item2;

                if (needNP)
                    messages.AddRange(new NPMessage(objectBox).CreateNP(pageNum, Category));

                new MultiProcessor(messages, objectBox).AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}