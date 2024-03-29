﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Commands.Message;
using TrimedBot.Core.Commands.Post;
using TrimedBot.Core.Commands.Post.Add;
using TrimedBot.Core.Commands.Post.Edit;
using TrimedBot.Core.Commands.Service.Settings;
using TrimedBot.Core.Commands.User.All;
using TrimedBot.Core.Commands.User.Manager.Message;
using TrimedBot.Core.Commands.User.Manager.Request;
using TrimedBot.Core.Commands.User.Manager.Settings;
using TrimedBot.Core.Commands.User.Member;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.Core.Commands.Service.Tags;
using TrimedBot.Core.Commands.Service.Channels;
using TrimedBot.Core.Commands.Service.Token;
using TrimedBot.Core.Commands.User.All.BlockedTags;

namespace TrimedBot.Core.Classes.Responses.ResponseTypes
{
    public class MessageInput : Input
    {
        public ObjectBox objectBox;
        public DAL.Entities.User user;
        public Message message;

        public MessageInput(ObjectBox objectBox, Message message) : base(objectBox)
        {
            this.objectBox = objectBox;
            user = objectBox.User;
            this.message = message;
        }

        public async Task ResponseCommand(List<Func<Task>> cmds, string command)
        {
            objectBox.IsNeedDeleteTemps = true;
            switch (command)
            {
                case "/start":
                    cmds.Add(new StartCommand(objectBox).Do);
                    break;
                case "/addvideo":
                case "add new video":
                    cmds.Add(new AddNewPostCommand(objectBox).Do);
                    break;
                case "/myvideos":
                case "my videos":
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
                case "tags":
                case "/tags":
                    cmds.Add(new TagsCommand(1, objectBox).Do);
                    break;
                case "channels":
                case "/channels":
                    cmds.Add(new ChannelsCommand(objectBox).Do);
                    break;
                case "send message to all":
                case "/SendMessageToAll":
                    cmds.Add(new GetInSendMessageToAllSectionCommand(objectBox).Do);
                    break;
                case "send message to admins":
                case "/SendMessageToAdmins":
                    cmds.Add(new GetInSendMessageToAdminsSectionCommand(objectBox).Do);
                    break;
                case "/commands":
                case "/help":
                    cmds.Add(new SendHelpCommand(objectBox).Do);
                    break;
                case "token":
                case "/token":
                    cmds.Add(new TokenCommand(objectBox).Do);
                    break;
                case "/census":
                case "census":
                    cmds.Add(new CensusCommand(objectBox).Do);
                    break;
                case "/Blockedtags":
                case "blocked tags":
                    cmds.Add(new BlockedTagsCommand(objectBox, 1).Do);
                    break;
                default:
                    cmds.Add(new NotfoundCommand(objectBox).Do);
                    break;
            }
        }

        public async Task ResponseVideo(List<Func<Task>> cmds, Video video, string caption)
        {
            switch (user.UserState)
            {
                case UserState.AddMedia_SendMedia:
                    cmds.Add(new AddMediaRecieveVideoCommand(objectBox, video).Do);
                    break;
                case UserState.EditMedia_Video:
                    cmds.Add(new EditMediaChangeVideoCommand(objectBox, video).Do);
                    break;
                case UserState.Send_Message_ToSomeone:
                    cmds.Add(new SendVideoToSomeOneCommand(objectBox, video, caption).Do);
                    break;
                case UserState.Send_Message_ToAll:
                    cmds.Add(new SendVideoToAllCommand(objectBox, video, message.Caption).Do);
                    break;
            }
        }

        public async Task ResponseMessage(List<Func<Task>> cmds, Message message)
        {
            switch (user.UserState)
            {
                case UserState.AddMedia_SendTitle:
                    cmds.Add(new AddMediaRecieveTitleCommand(objectBox, message.Text).Do);
                    break;
                case UserState.AddMedia_SendCaption:
                    cmds.Add(new AddMediaRecieveCaptionCommand(objectBox, message.Text).Do);
                    break;
                case UserState.EditMedia_Title:
                    cmds.Add(new EditMediaChangeTitleCommand(objectBox, message.Text).Do);
                    break;
                case UserState.EditMedia_Caption:
                    cmds.Add(new EditMediaChangeCaptionCommand(objectBox, message.Text).Do);
                    break;
                case UserState.Settings_Menu:
                    cmds.Add(new SettingsMenuCommand(objectBox, message.Text).Do);
                    break;
                case UserState.Settings_PerMemberAdsPrice:
                case UserState.Settings_BasicAdsPrice:
                case UserState.Settings_NumberOfAdsPerDay:
                    cmds.Add(new SetSettingsCommand(objectBox, message.Text).Do);
                    break;
                case UserState.Send_Message_ToSomeone:
                    cmds.Add(new SendMessageToSomeOneCommand(objectBox, message).Do);
                    break;
                case UserState.Send_Message_ToAll:
                    cmds.Add(new SendMessageToAllCommand(objectBox, message).Do);
                    break;
                case UserState.Send_Message_ToAdmins:
                    cmds.Add(new SendMessageToAdminsCommand(objectBox, message).Do);
                    break;
                case UserState.Add_General_Tag:
                    cmds.Add(new AddTagCommand(objectBox, message.Text).Do);
                    break;
            }
        }

        public async Task ResponseCancel(List<Func<Task>> cmds)
        {
            await new CancelCommand(objectBox).Do();
        }

        public async Task Response(List<Func<Task>> cmds, Message message)
        {
            switch (message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    string command = message.Text.ToLower();
                    if (command == "/cancel" || command == "cancel") { await ResponseCancel(cmds); }
                    else if (user.UserState == UserState.NoWhere) { await ResponseCommand(cmds, command); }
                    else await ResponseMessage(cmds, message);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    await ResponseVideo(cmds, message.Video, message.Caption);
                    break;
                //case Telegram.Bot.Types.Enums.MessageType.Photo:
                //    await ResponsePhoto(cmds, message.Photo, message.Caption);
                //    break;
                default:
                    await ResponseOther(cmds, message);
                    break;
            }
        }

        public void ResponseNotAvailable()
        {
            new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.ChatId,
                Text = Sentences.Bot_Not_Available
            }.AddThisMessageToService(objectBox.Provider);
        }

        public override async Task Action(List<Func<Task>> cmds)
        {
            if (objectBox.Settings.IsResponsingAvailable)
                await Response(cmds, message);
            else
                ResponseNotAvailable();
        }

        //Idk why but just don't delete it
        private async Task ResponsePhoto(List<Func<Task>> cmds, PhotoSize[] photo, string caption)
        {
            switch (user.UserState)
            {
                case UserState.Send_Message_ToSomeone:
                    cmds.Add(new SendPhotoToSomeOneCommand(objectBox, photo, caption).Do);
                    break;
                case UserState.Send_Message_ToAll:
                    cmds.Add(new SendPhotoToAllCommand(objectBox, message.Photo, message.Caption).Do);
                    break;
            }
        }

        private async Task ResponseOther(List<Func<Task>> cmds, Message message)
        {
            switch (user.UserState)
            {
                case UserState.Send_Message_ToSomeone:
                    cmds.Add(new SendMessageToSomeOneCommand(objectBox, message).Do);
                    break;
                case UserState.Send_Message_ToAll:
                    cmds.Add(new SendMessageToAllCommand(objectBox, message).Do);
                    break;
            }
        }
    }
}
