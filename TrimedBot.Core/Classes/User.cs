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
    public static class UserExtensions
    {
        public static void UpdateUserInfo(this ObjectBox objectBox)
        {
            objectBox.IsUserInfoChanged = true;
        }

        public static async Task UpdateDatabaseUserInfo(this ObjectBox objectBox)
        {
            if (objectBox.IsUserInfoChanged)
            {
                var userServices = objectBox.Provider.GetRequiredService<IUser>();
                userServices.Update(objectBox.User);
                await userServices.SaveAsync();
            }
        }
    }
}
