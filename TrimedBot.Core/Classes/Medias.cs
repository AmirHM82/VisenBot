using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes.Channel;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class Medias
    {
        public ObjectBox objectBox;

        public Medias(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task<Tuple<List<Processor>, bool>> GetPrivate(int pageNum)
        {
            bool needNP = false;
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            List<Processor> messages = new();

            if (pageNum > 0)
            {
                var count = await mediaServices.CountAsync(objectBox.User.Id);
                var medias = await mediaServices.GetMediasAsync(objectBox.User, pageNum);
                if (medias.Length > 0)
                {
                    for (int i = 0; i < medias.Length; i++)
                    {
                        messages.Add(new VideoResponseProcessor()
                        {
                            Text = $"{medias[i].Title}\n{medias[i].Caption}",
                            Keyboard = Keyboard.PrivateMediaKeyboard(medias[i].Id),
                            ReceiverId = objectBox.User.UserId,
                            IsDeletable = true,
                            Video = medias[i].FileId
                        });
                    }

                    //objectBox.User.UserLocation = UserLocation.SeeAddedVideos_Member;
                    objectBox.User.Temp = "SendPrivateMedias";
                    objectBox.UpdateUserInfo();
                    if (count > 5 || pageNum >= 2) needNP = true;
                }
                else
                {
                    messages.Add(new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "There is no videos.",
                        Keyboard = objectBox.Keyboard
                    });

                    //objectBox.User.UserLocation = UserLocation.NoWhere;
                    //objectBox.UpdateUserInfo();
                }
            }
            return new Tuple<List<Processor>, bool>(messages, needNP);
        }

        public async Task<Tuple<List<Processor>, bool>> GetPublic(int pageNum)
        {
            bool needNP = false;
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
            {
                if (pageNum > 0)
                {
                    var count = await mediaServices.CountAsync(objectBox.User.Id);
                    var medias = await mediaServices.GetNotConfirmedPostsAsync(pageNum);
                    if (medias.Length > 0)
                    {
                        for (int i = 0; i < medias.Length; i++)
                        {
                            StringBuilder tags = new StringBuilder();
                            if (medias[i].Tags is not null)
                                foreach (var item in medias[i].Tags)
                                {
                                    tags.Append($"{item.Name} ");
                                }

                            messages.Add(new VideoResponseProcessor()
                            {
                                ReceiverId = objectBox.User.UserId,
                                Video = medias[i].FileId,
                                Text = $"{medias[i].Title}\n{medias[i].Caption}\nTags: {tags}",
                                Keyboard = Keyboard.DeclinedPublicMediaKeyboard(medias[i].Id),
                                IsDeletable = true
                            });
                        }

                        //objectBox.User.UserLocation = objectBox.User.Access == Access.Admin
                        //   ? UserLocation.SeeAddedVideos_Admin
                        //   : UserLocation.SeeAddedVideos_Manager;
                        objectBox.User.Temp = "SendPublicMedias";
                        objectBox.UpdateUserInfo();
                        if (count > 5 || pageNum >= 2) needNP = true;
                    }
                    else
                    {
                        messages.Add(new TextResponseProcessor()
                        {
                            ReceiverId = objectBox.User.UserId,
                            Text = "No medias found.",
                            Keyboard = objectBox.Keyboard
                        });

                        objectBox.User.UserState = UserState.NoWhere;
                        objectBox.UpdateUserInfo();
                    }
                }
            }
            return new Tuple<List<Processor>, bool>(messages, needNP);
        }

        public async Task<List<Processor>> GetMediasForChannel()
        {
            List<Processor> messages = new List<Processor>();

            var mediaService = objectBox.Provider.GetRequiredService<IMedia>();
            var medias = await mediaService.GetConfirmedMedias();

            foreach (var m in medias)
            {
                StringBuilder tags = new StringBuilder();
                foreach (var item in m.Tags)
                {
                    tags.Append($"{item.Name} ");
                }

                string text = $"{m.Title} - {m.Caption}\nTags: {tags}";
                messages.Add(new ChannelVideoProcessor()
                {
                    ReceiverId = objectBox.ChatId,
                    Text = text,
                    Video = m.FileId
                });
            }
            return messages;
        }
    }
}