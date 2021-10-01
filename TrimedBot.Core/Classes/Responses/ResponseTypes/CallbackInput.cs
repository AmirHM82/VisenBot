using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Commands.Message;
using TrimedBot.Core.Commands.objectBox.User.Manager.Request;
using TrimedBot.Core.Commands.Post;
using TrimedBot.Core.Commands.Post.Delete;
using TrimedBot.Core.Commands.Post.Edit;
using TrimedBot.Core.Commands.User.Admin;
using TrimedBot.Core.Commands.User.Manager;
using TrimedBot.Core.Commands.User.Manager.Message;
using TrimedBot.Core.Commands.User.Manager.Request;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Commands.Service.Tags;
using TrimedBot.Core.Commands.User.All;
using TrimedBot.Core.Commands.Post.Properties;
using TrimedBot.Core.Commands.Post.Tag;
using TrimedBot.Core.Commands.Service.Channels;
using TrimedBot.Core.Commands.User.All.BlockedTags;

namespace TrimedBot.Core.Classes.Responses.ResponseTypes
{
    public class CallbackInput : Input
    {
        private ObjectBox objectBox;
        public CallbackQuery callbackQuery;

        public CallbackInput(ObjectBox objectBox, CallbackQuery callbackQuery) : base(objectBox)
        {
            this.objectBox = objectBox;
            this.callbackQuery = callbackQuery;
        }

        public override async Task Action(List<Func<Task>> cmds)
        {
            if (!objectBox.User.IsBanned) ResponseCallback(cmds, callbackQuery);
        }

        public void ResponseCallback(List<Func<Task>> cmds, CallbackQuery callbackQuery)
        {
            new CallbackQueryProcessor()
            {
                Id = callbackQuery.Id
            }.AddThisMessageToService(objectBox.Provider);

            var data = new Queue<string>(callbackQuery.Data.Split("/"));

            switch (data.Dequeue())
            {
                case CallbackSection.Post:
                    ResponsePostSection(cmds, data);
                    break;
                case CallbackSection.Admin:
                    ResponseAdminSection(cmds, data);
                    break;
                case CallbackSection.User:
                    ResponseUserSection(cmds, data);
                    break;
                case CallbackSection.Tag:
                    ResponseTagSection(cmds, data);
                    break;
                case CallbackSection.Cancel:
                    ResponseCancelSection(cmds, data);
                    break;
                case CallbackSection.Channel:
                    ResponseChannelSection(cmds, data);
                    break;
            }
        }

        public void ResponsePostSection(List<Func<Task>> cmds, Queue<string> data)
        {
            switch (data.Dequeue())
            {
                case CallbackSection.Edit:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Title:
                            cmds.Add(new GetInEditMediaChangeTitleSectionCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Caption:
                            cmds.Add(new GetInEditMediaChangeCaptionSectionCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Video:
                            cmds.Add(new GetInEditMediaChangeVideoSectionCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                    }
                    break;
                case CallbackSection.Delete:
                    cmds.Add(new DeletePostCommand(objectBox, callbackQuery.Message.MessageId, data.Dequeue()).Do);
                    break;
                case CallbackSection.Confirm:
                    cmds.Add(new ConfirmPostCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                    break;
                case CallbackSection.Decline:
                    cmds.Add(new DeclinePostCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                    break;
                case CallbackSection.Properties:
                    cmds.Add(new PostPropertiesCommand(Guid.Parse(data.Dequeue()), objectBox).Do);
                    break;
                case CallbackSection.Tag:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Add:
                            cmds.Add(new AddPostsTagCommand(objectBox, Guid.Parse(data.Dequeue())).Do);
                            break;
                        case CallbackSection.Delete:
                            cmds.Add(new DeletePostsTagCommand(objectBox, int.Parse(data.Dequeue()), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Next:
                        case CallbackSection.Previous:
                            cmds.Add(new PostsTagsCommand(objectBox).Do);
                            break;
                    }
                    break;
                case CallbackSection.Next:
                case CallbackSection.Previous:
                    cmds.Add(new MediasNPCommand(objectBox, int.Parse(data.Dequeue()), CallbackSection.Post).Do);
                    break;
            }
        }

        public void ResponseAdminSection(List<Func<Task>> cmds, Queue<string> data)
        {
            switch (data.Dequeue())
            {
                case CallbackSection.Request:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Accept:
                            cmds.Add(new AcceptAdminRequestCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Refuse:
                            cmds.Add(new RefuseAdminRequestCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Next:
                        case CallbackSection.Previous:
                            cmds.Add(new SendAdminRequestsCommand(objectBox, int.Parse(data.Dequeue())).Do);
                            break;
                    }
                    break;
                case CallbackSection.Delete:
                    cmds.Add(new DeleteAdminCommand(objectBox, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                    break;
                case CallbackSection.Add:
                    cmds.Add(new AddAdminCommand(objectBox, data.Dequeue()).Do);
                    break;
                case CallbackSection.Next:
                case CallbackSection.Previous:
                    cmds.Add(new AdminsCommand(objectBox, int.Parse(data.Dequeue())).Do);
                    break;
                default:
                    break;
            }
        }

        public void ResponseUserSection(List<Func<Task>> cmds, Queue<string> data)
        {
            switch (data.Dequeue())
            {
                case CallbackSection.Ban:
                    cmds.Add(new BanUserCommand(objectBox, Guid.Parse(data.Dequeue()), callbackQuery.Message.MessageId).Do);
                    break;
                case CallbackSection.Unban:
                    cmds.Add(new BanUserCommand(objectBox, Guid.Parse(data.Dequeue()), callbackQuery.Message.MessageId).UnDo);
                    break;
                case CallbackSection.Send:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Message:
                            cmds.Add(new GetInSendMessageToSomeOneCommand(objectBox, data.Dequeue()).Do);
                            break;
                    }
                    break;
                case CallbackSection.Tag:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Add:
                            cmds.Add(new AddBlockedTag(objectBox).Do);
                            break;
                        case CallbackSection.Delete:
                            cmds.Add(new DeleteUserBlockedTag(objectBox, int.Parse(data.Dequeue())).Do);
                            break;
                        case CallbackSection.Next:
                        case CallbackSection.Previous:
                            cmds.Add(new BlockedTagsCommand(objectBox, int.Parse(data.Dequeue())).Do);
                            break;
                    }
                    break;
            }
        }

        public void ResponseTagSection(List<Func<Task>> cmds, Queue<string> data)
        {
            switch (data.Dequeue())
            {
                case CallbackSection.Add:
                    cmds.Add(new GetInAddTagSectionCommand(objectBox).Do);
                    break;
                case CallbackSection.Delete:
                    cmds.Add(new DeleteTagCommand(objectBox, int.Parse(data.Dequeue()), callbackQuery.Message.MessageId).Do);
                    break;
                case CallbackSection.Next:
                case CallbackSection.Previous:
                    cmds.Add(new TagsCommand(int.Parse(data.Dequeue()), objectBox).Do);
                    break;
            }
        }

        public void ResponseCancelSection(List<Func<Task>> cmds, Queue<string> data)
        {
            cmds.Add(new CancelCommand(objectBox).Do);
        }

        public void ResponseChannelSection(List<Func<Task>> cmds, Queue<string> data)
        {
            switch (data.Dequeue())
            {
                case CallbackSection.Add:
                    cmds.Add(new AddChannelCommand(objectBox).Do);
                    break;
                case CallbackSection.Delete:
                    cmds.Add(new DeleteChannelCommand(objectBox, int.Parse(data.Dequeue()), callbackQuery.Message.MessageId).Do);
                    break;
            }
        }
    }
}
