using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrimedBot.Core.Commands.Message;
using TrimedBot.Core.Commands.Post;
using TrimedBot.Core.Commands.Post.Add;
using TrimedBot.Core.Commands.Post.Edit;
using TrimedBot.Core.Commands.Service.Settings;
using TrimedBot.Core.Commands.Service.Temp;
using TrimedBot.Core.Commands.User.All;
using TrimedBot.Core.Commands.User.Manager.Message;
using TrimedBot.Core.Commands.User.Manager.Request;
using TrimedBot.Core.Commands.User.Manager.Settings;
using TrimedBot.Core.Commands.User.Member;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes;
using Microsoft.Extensions.Caching.Distributed;

namespace TrimedBot.Core.Classes.Responses.ResponseTypes
{
    public class MessageInput : Input
    {
        private ObjectBox objectBox;
        private DAL.Entities.User user;
        protected BotServices _bot;
        public Message message;

        public MessageInput(ObjectBox objectBox, Message message) : base(objectBox)
        {
            this.objectBox = objectBox;
            _bot = objectBox.Provider.GetRequiredService<BotServices>();
            user = objectBox.User;
            this.message = message;
        }

        public async Task ResponseCommand(string command)
        {
            List<Func<Task>> cmds = new();
            switch (command)
            {
                case "/start":
                    cmds.Add(new StartCommand(objectBox).Do);
                    break;
                case "add new video":
                    cmds.Add(new AddNewPostCommand(objectBox).Do);
                    break;
                case "/myvideos":
                case "my videos":
                    //cmds.Add(new SendPrivateMediasCommand(objectBox, 1).Do);
                    //cmds.Add(new SendNPMessageCommand(objectBox, 1, CallbackSection.Post).Do);
                    cmds.Add(new MyVideosCommand(objectBox, 1).Do);
                    break;
                case "send admin request":
                case "/sendadminrequest":
                    cmds.Add(new SendAdminRequestCommand(objectBox).Do);
                    break;
                case "admin requests":
                case "/adminrequests":
                    cmds.Add(new SendAdminRequestsCommand(objectBox, 1).Do);
                    break;
                case "admins":
                case "/admins":
                    cmds.Add(new AdminsCommand(objectBox, 1).Do);
                    break;
                case "posts":
                case "/posts":
                    //cmds.Add(new SendPublicMediasCommand(objectBox, 1).Do);
                    //cmds.Add(new SendNPMessageCommand(objectBox, 1, CallbackSection.Post).Do);
                    cmds.Add(new PostsCommand(objectBox).Do);
                    break;
                case "search in posts":
                case "/searchinposts":
                    cmds.Add(new GetInSearchInPostsSectionCommand(objectBox).Do);
                    break;
                case "search in users":
                case "/searchinusers":
                    cmds.Add(new OpenSearchInUsersSectionCommand(objectBox).Do);
                    break;
                case "settings":
                case "/Settings":
                    cmds.Add(new OpenSettingsMenuCommand(objectBox).Do);
                    break;
                case "send message to all":
                case "/SendMessageToAll":
                    cmds.Add(new GetInSendMessageToAllSectionCommand(objectBox).Do);
                    break;
                case "/commands":
                case "/help":
                    cmds.Add(new SendHelpCommand(objectBox).Do);
                    break;
                default:
                    cmds.Add(new NotfoundCommand(objectBox).Do);
                    break;
            }
            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }

        public async Task ResponseVideo(Video video, string caption)
        {
            List<Func<Task>> cmds = new List<Func<Task>>();
            switch (user.UserPlace)
            {
                case UserPlace.AddMedia_SendMedia:
                    cmds.Add(new AddMediaRecieveVideoCommand(objectBox, video).Do);
                    break;
                case UserPlace.EditMedia_Video:
                    cmds.Add(new EditMediaChangeVideoCommand(objectBox, video).Do);
                    break;
                case UserPlace.Send_Message_ToSomeone:
                    cmds.Add(new DeleteTempMessagesCommand(objectBox).Do);
                    cmds.Add(new SendVideoToSomeOneCommand(objectBox, video, caption).Do);
                    break;
                case UserPlace.Send_Message_ToAll:
                    cmds.Add(new SendVideoToAllCommand(objectBox, video, message.Caption).Do);
                    break;
                default:
                    break;
            }
            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }

        public async Task ResponseMessage(Message message)
        {
            List<Func<Task>> cmds = new List<Func<Task>>();
            switch (user.UserPlace)
            {
                case UserPlace.AddMedia_SendTitle:
                    cmds.Add(new AddMediaRecieveTitleCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.AddMedia_SendCaption:
                    cmds.Add(new AddMediaRecieveCaptionCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.EditMedia_Title:
                    cmds.Add(new EditMediaChangeTitleCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.EditMedia_Caption:
                    cmds.Add(new EditMediaChangeCaptionCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.Settings_Menu:
                    cmds.Add(new SettingsMenuCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.Settings_PerMemberAdsPrice:
                case UserPlace.Settings_BasicAdsPrice:
                case UserPlace.Settings_NumberOfAdsPerDay:
                    cmds.Add(new SetSettingsCommand(objectBox, message.Text).Do);
                    break;
                case UserPlace.Send_Message_ToSomeone:
                    cmds.Add(new DeleteTempMessagesCommand(objectBox).Do);
                    cmds.Add(new SendMessageToSomeOneCommand(objectBox, message).Do);
                    break;
                case UserPlace.Send_Message_ToAll:
                    cmds.Add(new SendMessageToAllCommand(objectBox, message).Do);
                    break;
            }
            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }

        public async Task ResponseCancel()
        {
            if (user.UserPlace != UserPlace.NoWhere)
            {
                List<Func<Task>> cmds = new List<Func<Task>>();
                cmds.Add(new DeleteTempMessagesCommand(objectBox).Do);
                cmds.Add(new CancelCommand(objectBox).Do);
                foreach (var cmd in cmds)
                {
                    await cmd();
                }
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = "You are in home page.",
                Keyboard = objectBox.Keyboard
            }.AddThisMessageToService(objectBox.Provider);
        }

        public override async Task Action()
        {
            switch (message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    string command = message.Text.ToLower();
                    if (command == "/cancel" || command == "cancel") { await ResponseCancel(); return; }
                    if (user.UserPlace == UserPlace.NoWhere) { await ResponseCommand(command); return; }
                    await ResponseMessage(message);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    await ResponseVideo(message.Video, message.Caption);
                    break;
                //case Telegram.Bot.Types.Enums.MessageType.Photo:
                //    await ResponsePhoto(message.Photo, message.Caption);
                //    break;
                default:
                    await ResponseOther(message);
                    break;
            }
        }

        private async Task ResponsePhoto(PhotoSize[] photo, string caption)
        {
            List<Func<Task>> cmds = new List<Func<Task>>();
            switch (user.UserPlace)
            {
                case UserPlace.Send_Message_ToSomeone:
                    cmds.Add(new DeleteTempMessagesCommand(objectBox).Do);
                    cmds.Add(new SendPhotoToSomeOneCommand(objectBox, photo, caption).Do);
                    break;
                case UserPlace.Send_Message_ToAll:
                    cmds.Add(new SendPhotoToAllCommand(objectBox, message.Photo, message.Caption).Do);
                    break;
            }
            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }

        private async Task ResponseOther(Message message)
        {
            List<Func<Task>> cmds = new List<Func<Task>>();
            switch (user.UserPlace)
            {
                case UserPlace.Send_Message_ToSomeone:
                    cmds.Add(new DeleteTempMessagesCommand(objectBox).Do);
                    cmds.Add(new SendMessageToSomeOneCommand(objectBox, message).Do);
                    break;
                case UserPlace.Send_Message_ToAll:
                    cmds.Add(new SendMessageToAllCommand(objectBox, message).Do);
                    break;
            }
            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }
    }
}
