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
    public class Admins
    {
        public ObjectBox objectBox;

        public Admins(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Send(int pageNumber)
        {
            if (objectBox.User.Access == Access.Manager)
            {
                if (pageNumber > 0)
                {
                    var userServices = objectBox.Provider.GetRequiredService<IUser>();
                    DAL.Entities.User[] admins = await userServices.GetAdminsAsync(pageNumber);
                    if (admins.Length > 0)
                    {
                        List<Processor> messages = new();
                        for (int i = 0; i < admins.Length; i++)
                        {
                            messages.Add(new TextResponseProcessor()
                            {
                                ReceiverId = objectBox.User.UserId,
                                Text = $"UserName: {admins[i].UserName} \n Start date: {admins[i].StartDate}",
                                Keyboard = Keyboard.AdminDelete(admins[i].Id),
                                IsDeletable = true
                            });
                        }
                        new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                        objectBox.User.UserLocation = UserLocation.SeeAdmins_Manager;
                        objectBox.UpdateUserInfo();
                    }
                    else new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Admins not found"
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
            else new TextResponseProcessor()
            {
                ReceiverId = objectBox.User.UserId,
                Text = Sentences.Access_Denied
            }.AddThisMessageToService(objectBox.Provider);
        }

        public async Task<Tuple<List<Processor>, bool>> CreateSendMessages(int pageNumber)
        {
            bool needNP = false;
            List<Processor> messages = new();
            if (objectBox.User.Access == Access.Manager)
            {
                if (pageNumber > 0)
                {
                    var userServices = objectBox.Provider.GetRequiredService<IUser>();
                    DAL.Entities.User[] admins = await userServices.GetAdminsAsync(pageNumber);
                    if (admins.Length > 0)
                    {
                        for (int i = 0; i < admins.Length; i++)
                        {
                            string userInfo = "";
                            if (admins[i].UserName != null || admins[i].UserName != "") userInfo += $"Username: {admins[i].UserName}\n";
                            if (admins[i].FirstName != null && admins[i].FirstName != "") userInfo += $"Firstname: {admins[i].FirstName}\n";
                            if (admins[i].LastName != null && admins[i].LastName != "") userInfo += $"Lastname: {admins[i].LastName}";
                            messages.Add(new TextResponseProcessor()
                            {
                                ReceiverId = objectBox.User.UserId,
                                Text = $"{userInfo} \n Start date: {admins[i].StartDate}",
                                Keyboard = Keyboard.AdminDelete(admins[i].Id),
                                IsDeletable = true
                            });
                        }

                        objectBox.User.UserLocation = UserLocation.SeeAdmins_Manager;
                        objectBox.UpdateUserInfo();
                        needNP = true;
                    }
                    else messages.Add(new TextResponseProcessor()
                    {
                        ReceiverId = objectBox.User.UserId,
                        Text = "Admins not found"
                    });
                }
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
