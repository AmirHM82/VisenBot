using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes.Channel;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class Media
    {
        public ObjectBox objectBox;

        public Media(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public void SendPrivate(DAL.Entities.Media media)
        {
            if (media != null)
            {
                new VideoResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = $"{media.Title}\n{media.Caption}",
                    Keyboard = objectBox.User.Access == Access.Member ?
                    Keyboard.PrivateMediaKeyboard(media.Id) :
                    Keyboard.PublicPostProperties(media.Id, false),
                    IsDeletable = true,
                    Video = media.FileId
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No posts found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public async Task SendPrivate(Guid postId, bool HasCancel)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var media = await mediaServices.FindAsync(postId);

            var state = objectBox.User.LastUserState;
            new VideoResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = Keyboard.PrivatePostProperties(postId, HasCancel),
                Text = $"{media.Title} - {media.Caption}",
                IsDeletable = true,
                Video = media.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public async Task SendPublic(Guid postId, bool HasCancel)
        {
            var mediaServices = objectBox.Provider.GetRequiredService<IMedia>();
            var media = await mediaServices.FindAsync(postId);

            var state = objectBox.User.LastUserState;
            new VideoResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Keyboard = Keyboard.PublicPostProperties(postId, HasCancel),
                Text = $"{media.Title} - {media.Caption}",
                IsDeletable = true,
                Video = media.FileId
            }.AddThisMessageToService(objectBox.Provider);
        }

        public void SendPublic(DAL.Entities.Media media)
        {
            if (media != null)
            {
                InlineKeyboardMarkup key;
                if (!media.IsConfirmed)
                    key = Keyboard.DeclinedPublicMediaKeyboard(media.Id);
                else
                    key = Keyboard.ConfirmedPublicMediaKeyboard(media.Id);

                new VideoResponseProcessor(objectBox)
                {
                    ReceiverId = objectBox.User.UserId,
                    Video = media.FileId,
                    Text = $"{media.Title}\n{media.Caption}",
                    Keyboard = key,
                    IsDeletable = true
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = "No posts found",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        //Posts for admins channel have different keyboard, that's why bool is there
        public async Task SendToOtherChannels(DAL.Entities.Media media)
        {
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channels = await channelService.GetOtherChannelsAsync();

            if (channels.Count > 0)
            {
                string tags = "";
                if (media.Tags != null)
                    foreach (var t in media.Tags)
                    {
                        tags += $"{t.Name}, ";
                    }

                string text;
                if (tags != "") text = $"{media.Title} - {media.Caption}\nTags: {tags}";
                else text = $"{media.Title} - {media.Caption}";

                foreach (var c in channels)
                {
                    new ChannelVideoProcessor(objectBox)
                    {
                        Channel = c,
                        Text = text,
                        Video = media.FileId
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }

        public async Task SendToAdminChannels(DAL.Entities.Media media)
        {
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channels = await channelService.GetAdminChannelsAsync();

            if (channels.Count > 0)
            {
                string tags = "";
                if (media.Tags != null)
                    foreach (var t in media.Tags)
                    {
                        tags += $"{t.Name}, ";
                    }

                string text;
                if (tags != "") text = $"{media.Title} - {media.Caption}\nTags: {tags}";
                else text = $"{media.Title} - {media.Caption}";

                foreach (var c in channels)
                {
                    new ChannelVideoProcessor(objectBox)
                    {
                        Channel = c,
                        Text = text,
                        Video = media.FileId,
                        Keyboard = Keyboard.DeclinedPublicMediaKeyboard(media.Id),
                        IsDeletable = true
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }
    }
}