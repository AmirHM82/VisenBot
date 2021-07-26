using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.Message
{
    public class SendPublicMediasCommand : ICommand
    {
        //protected IUser userServices;
        private ObjectBox objectBox;
        private int pageNum;

        public SendPublicMediasCommand(ObjectBox objectBox, int pageNum)
        {
            //userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.objectBox = objectBox;
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
            {
                if (pageNum > 0)
                {
                    var medias = await mediaServices.GetNotConfirmedPostsAsync(pageNum);
                    if (medias.Length > 0)
                    {
                        //if (pageNum <= 0) pageNum = 1;
                        //if (medias.Length == 0) pageNum = 1;

                        List<Processor> messages = new();
                        for (int i = 0; i < medias.Length; i++)
                        {
                            messages.Add(new VideoResponseProcessor()
                            {
                                ReceiverId = objectBox.User.UserId,
                                Video = medias[i].FileId,
                                Text = $"{medias[i].Title}\n{medias[i].Caption}",
                                Keyboard = Keyboard.DeclinedPublicMediaKeyboard(medias[i].Id),
                                IsDeletable = true
                            });
                        }

                        new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);


                        objectBox.User.UserPlace = objectBox.User.Access == Access.Admin
                           ? UserPlace.SeeAddedVideos_Admin
                           : UserPlace.SeeAddedVideos_Manager;
                        //userServices.ChangeUserPlace(objectBox.User, a);
                        //await userServices.SaveAsync();
                    }
                    else new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "No medias found."
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
