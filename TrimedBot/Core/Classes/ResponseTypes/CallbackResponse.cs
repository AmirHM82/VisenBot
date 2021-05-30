using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Commands.Message;
using TrimedBot.Commands.objectBox.User.Manager.Request;
using TrimedBot.Commands.Post;
using TrimedBot.Commands.Post.Delete;
using TrimedBot.Commands.Post.Edit;
using TrimedBot.Commands.Service.Temp;
using TrimedBot.Commands.User.Admin;
using TrimedBot.Commands.User.Manager;
using TrimedBot.Commands.User.Manager.Message;
using TrimedBot.Commands.User.Manager.Request;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedBot.Database.Sections;

namespace TrimedBot.Core.Classes.ResponseTypes
{
    public class CallbackResponse
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        private Database.Models.User user;
        protected BotServices _bot;

        public CallbackResponse(IServiceProvider provider)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            user = objectBox.User;
        }

        public async Task Response(CallbackQuery callbackQuery)
        {
            List<Func<Task>> cmds = new();
            await _bot.AnswerCallbackQueryAsync(callbackQuery.Id);
            var data = new Queue<string>(callbackQuery.Data.Split("/"));

            switch (data.Dequeue())
            {
                case CallbackSection.Post:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Edit:
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                            switch (data.Dequeue())
                            {
                                case CallbackSection.Title:
                                    cmds.Add(new GetInEditMediaChangeTitleSectionCommand(provider, data.Dequeue()).Do);
                                    break;
                                case CallbackSection.Caption:
                                    cmds.Add(new GetInEditMediaChangeCaptionSectionCommand(provider, data.Dequeue()).Do);
                                    break;
                                case CallbackSection.Video:
                                    cmds.Add(new GetInEditMediaChangeVideoSectionCommand(provider, data.Dequeue()).Do);
                                    break;
                            }
                            break;
                        case CallbackSection.Delete:
                            cmds.Add(new DeletePostCommand(provider, callbackQuery.Message.MessageId, data.Dequeue()).Do);
                            break;
                        case CallbackSection.Confirm:
                            cmds.Add(new ConfirmPostCommand(provider, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Decline:
                            cmds.Add(new DeclinePostCommand(provider, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Next:
                        case CallbackSection.Previous:
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                            int pageNum = Int32.Parse(data.Dequeue());

                            if (user.UserPlace == UserPlace.SeeAddedVideos_Member)
                                cmds.Add(new SendPrivateMediasCommand(provider, pageNum).Do);
                            else if (user.UserPlace == UserPlace.SeeAddedVideos_Admin || user.UserPlace == UserPlace.SeeAddedVideos_Manager)
                                cmds.Add(new SendPublicMediasCommand(provider, pageNum).Do);

                            cmds.Add(new SendNPMessageCommand(provider, pageNum, CallbackSection.Post).Do);
                            break;
                    }
                    break;
                case CallbackSection.Admin:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Request:
                            switch (data.Dequeue())
                            {
                                case CallbackSection.Accept:
                                    cmds.Add(new AcceptAdminRequestCommand(provider, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                                    break;
                                case CallbackSection.Refuse:
                                    cmds.Add(new RefuseAdminRequestCommand(provider, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                                    break;
                                case CallbackSection.Next:
                                case CallbackSection.Previous:
                                    cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                                    if (user.UserPlace == UserPlace.SeeAdmins_Manager)
                                    {
                                        int pageNum = Int32.Parse(data.Dequeue());
                                        cmds.Add(new SendAdminRequestsCommand(provider, pageNum).Do);
                                        cmds.Add(new SendNPMessageCommand(provider, pageNum, $"{CallbackSection.Admin}/{CallbackSection.Request}").Do);
                                    }
                                    break;
                            }
                            break;
                        case CallbackSection.Delete:
                            cmds.Add(new DeleteAdminCommand(provider, data.Dequeue(), callbackQuery.Message.MessageId).Do);
                            break;
                        case CallbackSection.Add:
                            cmds.Add(new AddAdminCommand(provider, data.Dequeue()).Do);
                            break;
                        case CallbackSection.Next:
                        case CallbackSection.Previous:
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                            if (user.UserPlace == UserPlace.SeeAdmins_Manager)
                            {
                                int pageNum = Int32.Parse(data.Dequeue());
                                cmds.Add(new SendAdminsCommand(provider, pageNum).Do);
                                cmds.Add(new SendNPMessageCommand(provider, pageNum, CallbackSection.Admin).Do);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case CallbackSection.User:
                    switch (data.Dequeue())
                    {
                        case CallbackSection.Ban:
                            cmds.Add(new BanUserCommand(provider, Guid.Parse(data.Dequeue())).Do);
                            break;
                        case CallbackSection.Unban:
                            cmds.Add(new BanUserCommand(provider, Guid.Parse(data.Dequeue())).UnDo);
                            break;
                        case CallbackSection.Send:
                            switch (data.Dequeue())
                            {
                                case CallbackSection.Message:
                                    cmds.Add(new GetInSendMessageToSomeOneCommand(provider, data.Dequeue()).Do);
                                    break;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            foreach (var x in cmds)
            {
                await x();
                await Task.Delay(34);
            }
        }
    }
}
