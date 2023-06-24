using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Classes;

namespace TrimedBot.Core.Commands.User.Admin
{
    public class BanUserCommand : ICommand
    {
        protected IUser userServices;
        private ObjectBox objectBox;
        private Guid id;
        public int MessageId;

        public BanUserCommand(ObjectBox objectBox, Guid id, int messageId)
        {
            this.objectBox = objectBox;
            this.id = id;
            userServices = objectBox.Provider.GetRequiredService<IUser>();
            MessageId = messageId;
        }

        public async Task Do()
        {
            if (objectBox.User.Access != Access.Member)
            {
                var user = await userServices.FindAsync(id);
                if (!user.IsBanned)
                {
                    user.IsBanned = true;
                    userServices.Update(user);
                    await userServices.SaveAsync();

                    new EditInlineKeyboardProcessor(objectBox)
                    {
                        ReceiverId = objectBox.User.UserId,
                        MessageId = MessageId,
                        Keyboard = Keyboard.ManageUser(user.Id, user.UserId, user.Access, user.IsBanned)
                    }.AddThisMessageToService(objectBox.Provider);

                    new TextResponseProcessor(objectBox)
                    {
                        ReceiverId = user.UserId,
                        Text = Sentences.User_Banned
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }

        public async Task UnDo()
        {
            if (objectBox.User.Access != Access.Member)
            {
                var user = await userServices.FindAsync(id);
                if (user.IsBanned)
                {
                    user.IsBanned = false;
                    userServices.Update(user);
                    await userServices.SaveAsync();

                    new EditInlineKeyboardProcessor(objectBox)
                    {
                        ReceiverId = objectBox.User.UserId,
                        MessageId = MessageId,
                        Keyboard = Keyboard.ManageUser(user.Id, user.UserId, user.Access, user.IsBanned)
                    }.AddThisMessageToService(objectBox.Provider);

                    new TextResponseProcessor(objectBox)
                    {
                        ReceiverId = user.UserId,
                        Text = Sentences.User_Unbanned
                    }.AddThisMessageToService(objectBox.Provider);
                }
            }
        }
    }
}
