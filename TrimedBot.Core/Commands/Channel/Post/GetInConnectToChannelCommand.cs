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
    public class GetInConnectToChannelCommand : ICommand
    {
        private ObjectBox objectBox;
        private int messageId;

        public GetInConnectToChannelCommand(ObjectBox objectBox, int messageId)
        {
            this.objectBox = objectBox;
            this.messageId = messageId;
        }

        public async Task Do()
        {
            await new TempMessages(objectBox).AddUserTemp(new TempMessage   //Replace it with ChannelPosts class or maybe it's not needed
            {
                MessageId = messageId,
                //Type = DAL.Enums.TempType.Channel,
                ChatId = objectBox.Channel.ChatId
            });

            await new ChannelPosts(objectBox).Add(new ChannelPost
            {
                Channel = objectBox.Channel,
                MessageId = messageId,
                PostType = DAL.Enums.PostType.Temp
            });

            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channel = await channelService.FindAsync(objectBox.Channel.ChatId);
            if (channel is not null)
            {
                if (!channel.IsVerified)
                {
                    new TextResponseProcessor(objectBox)
                    {
                        IsDeletable = true,
                        ReceiverId = objectBox.Channel.ChatId,
                        Text = "Ok, now send the token."
                    }.AddThisMessageToService(objectBox.Provider);

                    objectBox.Channel.State = DAL.Enums.ChannelState.AddChannel;
                    objectBox.UpdateChannelInfo();
                }
                else new TextResponseProcessor(objectBox)
                {
                    IsDeletable = true,
                    ReceiverId = objectBox.ChatId,
                    Text = "This channel has beem added before"
                }.AddThisMessageToService(objectBox.Provider);
            }
            
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
