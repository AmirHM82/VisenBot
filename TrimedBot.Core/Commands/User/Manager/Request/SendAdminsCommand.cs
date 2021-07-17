using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Sections;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Manager.Request
{
    public class SendAdminsCommand : ICommand
    {
        private int pageNumber;
        protected IUser userServices;
        private ObjectBox objectBox;

        public SendAdminsCommand(ObjectBox objectBox, int pageNumber)
        {
            this.objectBox = objectBox;
            this.pageNumber = pageNumber;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
        }

        public async Task Do()
        {
            if (objectBox.User.Access == Access.Manager)
            {
                DAL.Entities.User[] admins = await userServices.GetAdminsAsync(pageNumber);
                if (admins.Length > 0)
                {
                    if (pageNumber <= 0) pageNumber = 1;
                    if (admins.Length == 0) pageNumber = 1;

                    List<Processor> messages = new();
                    for (int i = 0; i < admins.Length; i++)
                    {
                        messages.Add(new TextResponseProcessor()
                        {
                            RecieverId = objectBox.User.UserId,
                            Text = $"UserName: {admins[i].UserName} \n Start date: {admins[i].StartDate}",
                            Keyboard = Keyboard.AdminDelete(admins[i].Id),
                            IsDeletable = true
                        });
                    }
                    new MultiProcessor(messages).AddThisMessageToService(objectBox.Provider);

                    userServices.ChangeUserPlace(objectBox.User, UserPlace.SeeAdmins_Manager);
                    await userServices.SaveAsync();
                }
                else new TextResponseProcessor()
                {
                    RecieverId = objectBox.User.UserId,
                    Text = "Admins not found"
                };
            }
        }

        public Task UnDo()
        {
            throw new NotImplementedException();
        }
    }
}
