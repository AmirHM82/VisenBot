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

                        objectBox.User.UserPlace = UserPlace.SeeAdmins_Manager;
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

        public async Task<List<Processor>> CreateSendMessages(int pageNumber)
        {
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
                            messages.Add(new TextResponseProcessor()
                            {
                                ReceiverId = objectBox.User.UserId,
                                Text = $"UserName: {admins[i].UserName} \n Start date: {admins[i].StartDate}",
                                Keyboard = Keyboard.AdminDelete(admins[i].Id),
                                IsDeletable = true
                            });
                        }

                        objectBox.User.UserPlace = UserPlace.SeeAdmins_Manager;
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

            return messages;
        }
    }
}
