using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot.Types.ReplyMarkups;

using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class ObjectBox
    {
        public bool IsUserInfoChanged { get; set; }
        public User User { get; set; }
        public ReplyKeyboardMarkup Keyboard { get; set; }
        public Settings Settings { get; set; }

        public IServiceProvider Provider { get; set; }

        public ObjectBox(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        public async Task AssignUser(Telegram.Bot.Types.User user)
        {
            IUser userServices = Provider.GetRequiredService<IUser>();
            var NewOrFoundedUser = await userServices.FindOrAddAsync(user);

            bool IsChanged = false;
            if (NewOrFoundedUser.UserName != user.Username)
            {
                NewOrFoundedUser.UserName = user.Username;
                IsChanged = true;
            }
            else if (NewOrFoundedUser.FirstName != user.FirstName)
            {
                NewOrFoundedUser.FirstName = user.FirstName;
                IsChanged = true;
            }
            else if (NewOrFoundedUser.LastName != user.LastName)
            {
                NewOrFoundedUser.LastName = user.LastName;
                IsChanged = true;
            }

            if (IsChanged) IsUserInfoChanged = true;
            User = NewOrFoundedUser;
        }

        public async Task AssignSettings()
        {
            ISettings settingsServices = Provider.GetRequiredService<ISettings>();
            var settings = await settingsServices.GetSettings();
            if (settings == null)
            {
                settings = new Settings { BasicAdsPrice = 0, NumberOfAdsPerDay = 0, PerMemberAdsPrice = 0 };
                await settingsServices.Add(settings);
                await settingsServices.SaveAsync();
            }
            Settings = settings;
        }

        public void AssignKeyboard(Access access) => Keyboard = Classes.Keyboard.GetSpecificKeyboard(access);
    }
}
