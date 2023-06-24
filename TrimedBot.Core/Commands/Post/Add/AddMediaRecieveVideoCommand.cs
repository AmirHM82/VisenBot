using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using Media = TrimedBot.DAL.Entities.Media;

namespace TrimedBot.Core.Commands.Post.Add
{
    public class AddMediaRecieveVideoCommand : ICommand
    {
        private ObjectBox objectBox;
        protected IUser userServices;
        private Video video;

        public AddMediaRecieveVideoCommand(ObjectBox objectBox, Video video)
        {
            this.objectBox = objectBox;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            this.video = video;
        }

        public async Task Do()
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            //var cacheService = objectBox.Provider.GetRequiredService<CacheService>();

            Processor message;
            if (video == null)
            {
                message = new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "Please send a video.",
                    Keyboard = Keyboard.CancelKeyboard()
                };
            }
            else
            {
                message = new TextResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "It's done.",
                    Keyboard = objectBox.Keyboard
                };

                string[] TitleCaption = objectBox.User.Temp.Split("*");
                var media = new Media
                {
                    Title = TitleCaption[0],
                    Caption = TitleCaption[1],
                    FileId = video.FileId,
                    User = objectBox.User,
                    AddDate = DateTime.UtcNow
                };
                await mediaServices.AddAsync(media);
                await mediaServices.SaveAsync();

                //await cacheService.EditCachedMedias(new[] { media.Title, media.Caption }, new[] { media });
                await new Classes.Media(objectBox).SendToAdminChannels(media);
                await new Classes.Media(objectBox).SendToOtherChannels(media);

                objectBox.User.Temp = null;
                objectBox.User.UserState = UserState.NoWhere;
                objectBox.UpdateUserInfo();
            }
            message.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new Exception();
        }
    }
}
