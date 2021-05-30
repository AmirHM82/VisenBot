using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Commands;
using TrimedBot.Commands.Message;
using TrimedBot.Commands.objectBox.User.Manager.Request;
using TrimedBot.Commands.Post;
using TrimedBot.Commands.Post.Add;
using TrimedBot.Commands.Post.Delete;
using TrimedBot.Commands.Post.Edit;
using TrimedBot.Commands.Service.Settings;
using TrimedBot.Commands.Service.Temp;
using TrimedBot.Commands.User.Admin;
using TrimedBot.Commands.User.All;
using TrimedBot.Commands.User.Manager;
using TrimedBot.Commands.User.Manager.Message;
using TrimedBot.Commands.User.Manager.Request;
using TrimedBot.Commands.User.Manager.Settings;
using TrimedBot.Commands.User.Member;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.Database.Models;
using TrimedCore.Core.Classes;

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

            string[] splitedQuery = callbackQuery.Data.Split("/");
            string InputData = splitedQuery.LastOrDefault();

            switch (splitedQuery[0])
            {
                case "Post":
                    switch (splitedQuery[1])
                    {
                        case "Edit":
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                            switch (splitedQuery[2])
                            {
                                case "Title":
                                    cmds.Add(new GetInEditMediaChangeTitleSectionCommand(provider, InputData).Do);
                                    break;
                                case "Caption":
                                    cmds.Add(new GetInEditMediaChangeCaptionSectionCommand(provider, InputData).Do);
                                    break;
                                case "Video":
                                    cmds.Add(new GetInEditMediaChangeVideoSectionCommand(provider, InputData).Do);
                                    break;
                            }
                            break;
                        case "Delete":
                            cmds.Add(new DeletePostCommand(provider, callbackQuery.Message.MessageId, InputData).Do);
                            break;
                        case "Decline":
                        case "Confirm":
                            if (splitedQuery[1] == "Confirm")
                                cmds.Add(new ConfirmPostCommand(provider, InputData, callbackQuery.Message.MessageId).Do);
                            else if (splitedQuery[1] == "Decline")
                                cmds.Add(new DeclinePostCommand(provider, InputData, callbackQuery.Message.MessageId).Do);
                            break;
                        case "Next":
                        case "Previous":
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);

                            if (user.UserPlace == UserPlace.SeeAddedVideos_Member)
                                cmds.Add(new SendPrivateMediasCommand(provider, Int32.Parse(InputData)).Do);
                            else if (user.UserPlace == UserPlace.SeeAddedVideos_Admin || user.UserPlace == UserPlace.SeeAddedVideos_Manager)
                                cmds.Add(new SendPublicMediasCommand(provider, Int32.Parse(InputData)).Do);

                            cmds.Add(new SendNPMessageCommand(provider, Int32.Parse(InputData), "Post").Do);
                            break;
                    }
                    break;
                case "Admin":
                    switch (splitedQuery[1])
                    {
                        case "Request":
                            switch (splitedQuery[2])
                            {
                                case "Accept":
                                    cmds.Add(new AcceptAdminRequestCommand(provider, InputData, callbackQuery.Message.MessageId).Do);
                                    break;
                                case "Refuse":
                                    cmds.Add(new RefuseAdminRequestCommand(provider, InputData, callbackQuery.Message.MessageId).Do);
                                    break;
                                case "Next":
                                case "Previous":
                                    cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                                    if (user.UserPlace == UserPlace.SeeAdmins_Manager)
                                    {
                                        cmds.Add(new SendAdminRequestsCommand(provider, Int32.Parse(InputData)).Do);
                                        cmds.Add(new SendNPMessageCommand(provider, Int32.Parse(InputData), "Admin/Request").Do);
                                    }
                                    break;
                            }
                            break;
                        case "Delete":
                            cmds.Add(new DeleteAdminCommand(provider, InputData, callbackQuery.Message.MessageId).Do);
                            break;
                        case "Add":
                            cmds.Add(new AddAdminCommand(provider, InputData).Do);
                            break;
                        case "Next":
                        case "Previous":
                            cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                            if (user.UserPlace == UserPlace.SeeAdmins_Manager)
                            {
                                cmds.Add(new SendAdminsCommand(provider, Int32.Parse(InputData)).Do);
                                cmds.Add(new SendNPMessageCommand(provider, Int32.Parse(InputData), "Admin").Do);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "User":
                    switch (splitedQuery[1])
                    {
                        case "Ban":
                            cmds.Add(new BanUserCommand(provider, Guid.Parse(InputData)).Do);
                            break;
                        case "Unban":
                            cmds.Add(new BanUserCommand(provider, Guid.Parse(InputData)).UnDo);
                            break;
                        case "Send":
                            switch (splitedQuery[2])
                            {
                                case "Message":
                                    cmds.Add(new GetInSendMessageToSomeOneCommand(provider, InputData).Do);
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
