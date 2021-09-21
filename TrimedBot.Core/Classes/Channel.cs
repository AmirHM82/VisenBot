using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes
{
    public static class Channel
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
}
