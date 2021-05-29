using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Services
{
    public class ObjectBox
    {
        public User User { get; set; }
        public ReplyKeyboardMarkup Keyboard { get; set; }
        public Settings Settings { get; set; }

        private IServiceProvider provider;
        protected IUser userServices;
        protected ISettings settingsServices;

        public ObjectBox(IServiceProvider provider)
        {
            this.provider = provider;
            userServices = provider.GetRequiredService<IUser>();
            settingsServices = provider.GetRequiredService<ISettings>();
        }

        public async Task AssignUser(Telegram.Bot.Types.User user)
        {
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
            var settings = await settingsServices.GetSettings();
            if (settings == null)
            {
                settings = new Settings { BasicAdsPrice = 0, NumberOfAdsPerDay = 0, PerMemberAdsPrice = 0 };
                await settingsServices.Add(settings);
                await settingsServices.SaveAsync();
            }
        }

        public void AssignKeyboard(Access access) => Keyboard = TrimedCore.Core.Classes.Keyboard.SpecificKeyboard(access);
    }
}
