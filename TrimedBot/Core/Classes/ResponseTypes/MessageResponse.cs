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
using TrimedBot.Database.Sections;
using TrimedCore.Core.Classes;

namespace TrimedBot.Core.Classes.ResponseTypes
{
    public class MessageResponse
    {
        private IServiceProvider provider;
        private ObjectBox objectBox;
        private Database.Models.User user;
        private ReplyKeyboardMarkup keyboard;
        protected BotServices _bot;

        public MessageResponse(IServiceProvider provider)
        {
            this.provider = provider;
            objectBox = provider.GetRequiredService<ObjectBox>();
            _bot = provider.GetRequiredService<BotServices>();
            user = objectBox.User;
            keyboard = objectBox.Keyboard;
        }

        public async Task Response(Message message)
        {
            List<Func<Task>> cmds = new List<Func<Task>>();
            if (message.Text.ToLower() == "/cancel" || message.Text.ToLower() == "cancel")
            {
                cmds.Add(new DeleteTempMessagesCommand(provider).Do);
                cmds.Add(new CancelCommand(provider).Do);
            }
            else
            {
                switch (user.UserPlace)
                {
                    case UserPlace.NoWhere:
                        switch (message.Text.ToLower())
                        {
                            case "/start":
                                cmds.Add(new StartCommand(provider).Do);
                                break;
                            case "add new video":
                                cmds.Add(new AddNewPostCommand(provider).Do);
                                break;
                            case "/myvideos":
                            case "my videos":
                                cmds.Add(new SendPrivateMediasCommand(provider, 1).Do);
                                cmds.Add(new SendNPMessageCommand(provider, 1, CallbackSection.Post).Do);
                                break;
                            case "send admin request":
                            case "/sendadminrequest":
                                cmds.Add(new SendAdminRequestCommand(provider).Do);
                                break;
                            case "admin requests":
                            case "/adminrequests":
                                cmds.Add(new SendAdminRequestsCommand(provider, 1).Do);
                                cmds.Add(new SendNPMessageCommand(provider, 1, $"{CallbackSection.Admin}/{CallbackSection.Request}").Do);
                                break;
                            case "admins":
                            case "/admins":
                                cmds.Add(new SendAdminsCommand(provider, 1).Do);
                                cmds.Add(new SendNPMessageCommand(provider, 1, CallbackSection.Admin).Do);
                                break;
                            case "posts":
                            case "/posts":
                                cmds.Add(new SendPublicMediasCommand(provider, 1).Do);
                                cmds.Add(new SendNPMessageCommand(provider, 1, CallbackSection.Post).Do);
                                break;
                            case "search in posts":
                            case "/searchinposts":
                                cmds.Add(new GetInSearchInPostsSectionCommand(provider).Do);
                                break;
                            case "search in users":
                            case "/searchinusers":
                                cmds.Add(new OpenSearchInUsersSectionCommand(provider).Do);
                                break;
                            case "settings":
                            case "/Settings":
                                cmds.Add(new OpenSettingsMenuCommand(provider).Do);
                                break;
                            case "send message to all":
                            case "/SendMessageToAll":
                                cmds.Add(new GetInSendMessageToAllSectionCommand(provider).Do);
                                break;
                            case "/commands":
                            case "/help":
                                cmds.Add(new SendHelpCommand(provider).Do);
                                break;
                        }
                        break;
                    case UserPlace.AddMedia_SendTitle:
                        cmds.Add(new AddMediaRecieveTitleCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.AddMedia_SendCaption:
                        cmds.Add(new AddMediaRecieveCaptionCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.AddMedia_SendMedia:
                        cmds.Add(new AddMediaRecieveVideoCommand(provider, message.Video).Do);
                        break;
                    case UserPlace.EditMedia_Title:
                        cmds.Add(new EditMediaChangeTitleCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.EditMedia_Caption:
                        cmds.Add(new EditMediaChangeCaptionCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.EditMedia_Video:
                        cmds.Add(new EditMediaChangeVideoCommand(provider, message.Video).Do);
                        break;
                    case UserPlace.Settings_Menu:
                        cmds.Add(new SettingsMenuCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.Settings_PerMemberAdsPrice:
                    case UserPlace.Settings_BasicAdsPrice:
                    case UserPlace.Settings_NumberOfAdsPerDay:
                        cmds.Add(new SetSettingsCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.Send_Message_ToSomeone:
                        cmds.Add(new SendMessageToSomeOneCommand(provider, message.Text).Do);
                        break;
                    case UserPlace.Send_Message_ToAll:
                        cmds.Add(new SendMessageToAllCommand(provider, message.Text).Do);
                        break;
                    default:
                        cmds.Add(new NotfoundCommand(provider).Do);
                        break;
                }
            }
            foreach (var x in cmds)
            {
                await x(); 
                await Task.Delay(34);
            }
        }
    }
}
