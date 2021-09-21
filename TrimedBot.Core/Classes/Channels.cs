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

namespace TrimedBot.Core.Classes
{
    public class Channels
    {
        private ObjectBox objectBox;

        public Channels(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task<List<Processor>> GetMessages()
        {
            List<Processor> result = new List<Processor>();
            var channelService = objectBox.Provider.GetRequiredService<IChannel>();
            var channels = await channelService.GetChannelsAsync();


            if (channels.Count > 0)
            {
                result.Add(new TextResponseProcessor()
                {
                    IsDeletable = true,
                    //Keyboard = Keyboard.CancelKeyboard(),
                    ReceiverId = objectBox.User.UserId,
                    Text = "Channels:"
                });

                foreach (var c in channels)
                {
                    string text = $"{c.Name}\nState: ";
                    if (c.IsVerified) text += "Verified";
                    else text += "Not Verified";

                    result.Add(new TextResponseProcessor()
                    {
                        IsDeletable = true,
                        Keyboard = Keyboard.Channel(c),
                        ReceiverId = objectBox.User.UserId,
                        Text = text
                    });
                }
            }
            else result.Add(new TextResponseProcessor()
            {
                IsDeletable = true,
                //Keyboard = Keyboard.CancelKeyboard(),
                ReceiverId = objectBox.User.UserId,
                Text = "There is no channel"
            });

            result.Add(new TextResponseProcessor()
            {
                IsDeletable = true,
                Keyboard = Keyboard.AddChannel(),
                ReceiverId = objectBox.User.UserId,
                Text = "Press add to add a new channel"
            });

            //objectBox.User.UserLocation = DAL.Enums.UserLocation.See_All_Tags;
            //objectBox.UpdateUserInfo();

            return result;
        }
    }
}
