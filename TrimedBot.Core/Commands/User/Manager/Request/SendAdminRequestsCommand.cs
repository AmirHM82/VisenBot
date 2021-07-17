using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes.Processors;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class SendAdminRequestsCommand : ICommand
    {
        private ObjectBox objectBox;
        protected ITempMessage tempMessageServices;
        protected IUser userServices;
        protected BotServices _bot;
        private int pageNum;

        public SendAdminRequestsCommand(ObjectBox objectBox, int pageNum)
        {
            this.objectBox = objectBox;
            tempMessageServices = objectBox.Provider.GetRequiredService<ITempMessage>();
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            _bot = objectBox.Provider.GetRequiredService<BotServices>();
            this.pageNum = pageNum;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var users = await userServices.GetUsersWithAdminRequestAsync(pageNum);
                if (users.Length != 0)
                {
                    if (pageNum <= 0) pageNum = 1;
                    if (users.Length == 0) pageNum = 1;

                    List<Processor> messages = new();
                    for (int i = 0; i < users.Length; i++)
                    {
                        messages.Add(new TextResponseProcessor()
                        {
                            RecieverId = objectBox.User.UserId,
                            Text = $"UserName: {users[i].UserName} \n Start date: {users[i].StartDate}",
                            Keyboard = Keyboard.AdminRequest(users[i].UserId),
                            IsDeletable = true
                        });
                    }
                    new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                    userServices.ChangeUserPlace(objectBox.User, UserPlace.SeeAdminRequests_Manager);
                    await userServices.SaveAsync();
                }
                else new TextResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    Text = "No request found."
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                RecieverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
