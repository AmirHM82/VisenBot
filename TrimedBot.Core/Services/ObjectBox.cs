using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TrimedBot.Core.Services
{
    public class ObjectBox
    {
        public bool IsUserInfoChanged { get; set; }
        public bool IsChannelInfoChanged { get; set; }

        public bool IsNeedDeleteTemps { get; set; } = false;
        public long ChatId { get; set; }
        public DAL.Entities.User User { get; set; }
        public Channel? Channel { get; set; }
        public ReplyKeyboardMarkup Keyboard { get; set; }
        public Settings Settings { get; set; }
        public ChatType? ChatType { get; set; }

        public IServiceProvider Provider { get; set; }

        public ObjectBox(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        public async Task AssignUser(Telegram.Bot.Types.User user, bool validateChatId = true)
        {
            IUser userServices = Provider.GetRequiredService<IUser>();
            var u = new DAL.Entities.User
            {
                Access = Access.Member,
                IsSentAdminRequest = false,
                IsBanned = false,
                StartDate = DateTime.UtcNow,
                UserId = user.Id,
                UserState = UserState.NoWhere,
                UserName = user.Username
            };
            var NewOrFoundedUser = await userServices.FindOrAddAsync(u);

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
            if (validateChatId)
                ChatId = NewOrFoundedUser.UserId;
        }

        public async Task AssignChannel(Chat chat, bool validateChatId = true)
        {
            var channelServices = Provider.GetRequiredService<IChannel>();
            var channel = new Channel
            {
                ChatId = chat.Id,
                Name = chat.Title
            };
            var NewOrFoundedUser = await channelServices.FindOrAddAsync(channel);

            bool IsChanged = false;
            if (NewOrFoundedUser.Name != chat.Title)
            {
                NewOrFoundedUser.Name = chat.Title;
                IsChanged = true;
            }

            if (IsChanged) IsChannelInfoChanged = true;
            Channel = NewOrFoundedUser;
            if (validateChatId)
                ChatId = NewOrFoundedUser.ChatId;
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
