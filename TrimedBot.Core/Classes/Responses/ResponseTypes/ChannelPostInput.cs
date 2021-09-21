using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Commands.Channel.Post;
using TrimedBot.Core.Commands.Message;
using TrimedBot.Core.Commands.Service.Channels;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TrimedBot.Core.Classes.Responses.ResponseTypes
{
    public class ChannelPostInput : Input
    {
        public ChannelPostInput(ObjectBox objectBox, Message channelPost) : base(objectBox)
        {
            ObjectBox = objectBox;
            ChannelPost = channelPost;
            Channel = objectBox.Channel;
        }

        public ObjectBox ObjectBox { get; set; }
        public Message ChannelPost { get; }
        public DAL.Entities.Channel Channel { get; set; }

        public override async Task Action()
        {
            if (Channel.State == ChannelState.NoWhere) await ResponseCommand(ChannelPost.Text);
            else await ResponseMessage(ChannelPost);
        }

        public async Task ResponseCommand(string command)
        {
            ObjectBox.IsNeedDeleteTemps = true;
            List<Func<Task>> cmds = new();

            switch (command)
            {
                case "/connecttochannel":
                    cmds.Add(new GetInConnectToChannelCommand(ObjectBox, ChannelPost.MessageId).Do);
                    break;
                case "/sendallmedias":
                    cmds.Add(new SendAllMediasCommand(ObjectBox).Do);
                    break;
                case "/ping":
                    cmds.Add(new PingCommand(ObjectBox).Do);
                    break;
            }

            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }

        public async Task ResponseMessage(Message post)
        {
            List<Func<Task>> cmds = new();

            switch (Channel.State)
            {
                case ChannelState.AddChannel:
                    cmds.Add(new ConnectToChannelCommand(ObjectBox, post.Text, post.MessageId).Do);
                    break;
            }

            foreach (var cmd in cmds)
            {
                await cmd();
            }
        }
    }
}
