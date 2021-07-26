using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Message
{
    public class SendPrivateMediasCommand : ICommand
    {
        private int pageNum;
        private ObjectBox objectBox;

        public SendPrivateMediasCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            IMedia mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            //IUser userServices = objectBox.Provider.GetRequiredService<IUser>();

            if (pageNum > 0)
            {
                var medias = await mediaServices.GetMediasAsync(objectBox.User, pageNum);
                if (medias.Length != 0)
                {
                    //if (pageNum <= 0) pageNum = 1;
                    //if (medias.Length == 0) pageNum = 1;

                    for (int i = 0; i < medias.Length; i++)
                    {
                        new VideoResponseProcessor()
                        {
                            Text = $"{medias[i].Title}\n{medias[i].Caption}",
                            Keyboard = Keyboard.PrivateMediaKeyboard(medias[i].Id),
                            ReceiverId = objectBox.User.UserId,
                            IsDeletable = true,
                            Video = medias[i].FileId
                        }.AddThisMessageToService(objectBox.Provider);
                    }

                    objectBox.User.UserPlace = UserPlace.SeeAddedVideos_Member;
                    //userServices.Update(objectBox.User);
                    //await userServices.SaveAsync();
                }
                else new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "There is no videos."
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
