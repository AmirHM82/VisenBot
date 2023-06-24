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
    public static class ChannelExtensions
    {
        public static void UpdateChannelInfo(this ObjectBox objectBox)
        {
            objectBox.IsChannelInfoChanged = true;
        }

        public static async Task UpdateDatabaseChannelInfo(this ObjectBox objectBox)
        {
            if (objectBox.IsChannelInfoChanged)
            {
                var channelServices = objectBox.Provider.GetRequiredService<IChannel>();
                channelServices.Update(objectBox.Channel);
                await channelServices.SaveAsync();
            }
        }
    }

    public class Channel
    {
        private ObjectBox objectBox;

        public Channel(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task<List<Processor>> GetMessage(int channelId)
        {
            List<Processor> result = new List<Processor>();
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channel = await channelService.FindAsync(channelId);

            if (channel is not null)
            {
                string text = $"{channel.Name}\nType: {channel.Type}\nState: ";
                if (channel.IsVerified) text += "Verified";
                else text += "Not Verified";

                result.Add(new TextResponseProcessor(objectBox)
                {
                    IsDeletable = true,
                    Keyboard = Keyboard.ChannelProperties(channelId, true),
                    ReceiverId = objectBox.User.UserId,
                    Text = text
                });
            }
            else result.Add(new TextResponseProcessor(objectBox)
            {
                IsDeletable = true,
                ReceiverId = objectBox.User.UserId,
                Text = "Channel not found."
            });

            return result;
        }

        public List<Processor> GetTypes(int channelId)
        {
            List<Processor> result = new List<Processor>();

            result.Add(new TextResponseProcessor(objectBox)
            {
                IsDeletable = true,
                ReceiverId = objectBox.ChatId,
                Text = "Choose one of these types:",
                Keyboard = Keyboard.ChannelEnums(channelId)
            });

            return result;
        }
    }
}
