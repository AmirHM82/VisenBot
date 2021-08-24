using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Classes
{
    public class AdminRequests
    {
        public ObjectBox objectBox;

        public AdminRequests(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Send(int pageNum)
        {
            if (objectBox.User.Access == Access.Manager)
            {
                var userServices = objectBox.Provider.GetRequiredService<IUser>();
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
                            ReceiverId = objectBox.User.UserId,
                            Text = $"UserName: {users[i].UserName} \n Start date: {users[i].StartDate}",
                            Keyboard = Keyboard.AdminRequest(users[i].UserId),
                            IsDeletable = true
                        });
                    }
                    new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                    objectBox.User.UserLocation = UserLocation.SeeAdminRequests_Manager;
                    objectBox.UpdateUserInfo();
                }
                else new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "No request found."
                }.AddThisMessageToService(objectBox.Provider);
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied
            }.AddThisMessageToService(objectBox.Provider);
        }

        public async Task<Tuple<List<Processor>, bool>> CreateSendMessages(int pageNum)
        {
            bool needNP = false;
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Manager)
            {
                var userServices = objectBox.Provider.GetRequiredService<IUser>();
                var users = await userServices.GetUsersWithAdminRequestAsync(pageNum);
                if (users.Length != 0)
                {
                    for (int i = 0; i < users.Length; i++)
                    {
                        messages.Add(new TextResponseProcessor()
                        {
                            ReceiverId = objectBox.User.UserId,
                            Text = $"UserName: {users[i].UserName} \n Start date: {users[i].StartDate}",
                            Keyboard = Keyboard.AdminRequest(users[i].UserId),
                            IsDeletable = true
                        });
                    }

                    objectBox.User.UserLocation = UserLocation.SeeAdminRequests_Manager;
                    objectBox.UpdateUserInfo();

                    needNP = true;
                }
                else messages.Add(new TextResponseProcessor()
                {
                    ReceiverId = objectBox.User.UserId,
                    Text = "No request found."
                });
            }
            else messages.Add(new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied
            });

            return new Tuple<List<Processor>, bool>(messages, needNP);
        }
    }
}
