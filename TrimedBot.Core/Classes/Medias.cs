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

        public async Task SendPrivate(int pageNum)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();

            if (pageNum > 0)
            {
                var medias = await mediaServices.GetMediasAsync(objectBox.User, pageNum);
                if (medias.Length != 0)
                {
                    List<Processor> messages = new();
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
                    new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);
                    objectBox.User.UserPlace = UserPlace.SeeAddedVideos_Member;
                }
                else new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "There is no videos."
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public async Task<List<Processor>> GetPrivate(int pageNum)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            List<Processor> messages = new();

            if (pageNum > 0)
            {
                var medias = await mediaServices.GetMediasAsync(objectBox.User, pageNum);
                if (medias.Length != 0)
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
                    objectBox.User.UserPlace = UserPlace.SeeAddedVideos_Member;
                }
                else messages.Add(new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "There is no videos."
                });
            }

            return messages;
        }

        public async Task SendPublic(int pageNum)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
            {
                if (pageNum > 0)
                {
                    var medias = await mediaServices.GetNotConfirmedPostsAsync(pageNum);
                    if (medias.Length > 0)
                    {
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
                    }
                    else new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "No medias found."
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }

        public async Task<List<Processor>> GetPublic(int pageNum)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Admin || objectBox.User.Access == Access.Manager)
            {
                if (pageNum > 0)
                {
                    var medias = await mediaServices.GetNotConfirmedPostsAsync(pageNum);
                    if (medias.Length > 0)
                    {
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


                        objectBox.User.UserPlace = objectBox.User.Access == Access.Admin
                           ? UserPlace.SeeAddedVideos_Admin
                           : UserPlace.SeeAddedVideos_Manager;
                    }
                    else messages.Add(new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "No medias found."
                    });
                }
            }
            return messages;
        }
    }
}