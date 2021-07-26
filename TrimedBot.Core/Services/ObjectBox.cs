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
        public bool IsUserChanged { get; set; }
        private User _user;
        public User User { get => _user; set { _user = value; IsUserChanged = true; } }
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
            if (NewOrFoundedUser.UserName != user.Username)
            {
                NewOrFoundedUser.UserName = user.Username;
                userServices.Update(NewOrFoundedUser);
            }
            await userServices.SaveAsync();
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

        public async Task UpdateUserInfo()
        {
            if (IsUserChanged)
            {
                var userServices = Provider.GetRequiredService<IUser>();
                userServices.Update(User);
                await userServices.SaveAsync();
            }
        }
    }
}
