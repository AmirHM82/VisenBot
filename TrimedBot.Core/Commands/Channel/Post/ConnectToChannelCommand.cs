using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Commands.Channel.Post
{
    public class ConnectToChannelCommand : ICommand
    {
        private ObjectBox objectBox;
        private string Code;
        private int messageId;

        public ConnectToChannelCommand(ObjectBox objectBox, string code, int messageId)
        {
            this.objectBox = objectBox;
            Code = code;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            await new TempMessages(objectBox).Add(new TempMessage
            {
                MessageId = messageId,
                Type = DAL.Enums.TempType.Channel,
                ChatId = objectBox.Channel.ChatId
            });

            var tokenService = objectBox.Provider.GetRequiredService<IToken>();
            var token = await tokenService.GetToken();
            string text = "";
            long receiverId = 0;
            if (token is not null)
                if (token.Code == Code)
                {
                    objectBox.Channel.IsVerified = true;
                    objectBox.Channel.State = DAL.Enums.ChannelState.NoWhere;
                    objectBox.UpdateChannelInfo();

                    receiverId = objectBox.ChatId;
                    text = "The new channel has been connected successfully";

                    tokenService.Remove(token);
                    await tokenService.SaveAsync();

                    objectBox.IsNeedDeleteTemps = true;
                }
                else
                {
                    receiverId = objectBox.Channel.ChatId;
                    text = "This code doesn't exist";
                }
            else
            {
                receiverId = objectBox.Channel.ChatId;
                text = "There is no code in bot. Use /token in bot to create a new one.";
            }

            new TextResponseProcessor()
            {
                ReceiverId = receiverId,
                Text = text,
                IsDeletable = true
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
