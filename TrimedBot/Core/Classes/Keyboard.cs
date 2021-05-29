using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.Database.Models;

namespace TrimedCore.Core.Classes
{
    public static class Keyboard
    {
        public static ReplyKeyboardMarkup StartKeyboard_Member = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Send admin request") },
            new[] {new KeyboardButton("Search in posts") }
        }, true);

        public static ReplyKeyboardMarkup StartKeyboard_Admin = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Posts") },
            new[] {new KeyboardButton("Search in posts") }
        }, true);

        public static ReplyKeyboardMarkup StartKeyboard_Manager = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Posts") },
            new[] {new KeyboardButton("Admins") },
            new[] {new KeyboardButton("admin requests") },
            new[] {new KeyboardButton("Search in posts") },
            new[] {new KeyboardButton("Search in users") },
            new[] {new KeyboardButton("Settings") },
            new[] {new KeyboardButton("Send message to all") }
        }, true);

        public static ReplyKeyboardMarkup SettingsKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Ads properties")},
            new[] { new KeyboardButton("Cancel") }
        }, true);
        
        public static ReplyKeyboardMarkup AdsPropertiesKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("PerMemberAdsPrice") },
            new[] {new KeyboardButton("BasicAdsPrice") },
            new[] {new KeyboardButton("NumberOfAdsPerDay") },
            new[] { new KeyboardButton("Cancel") }
        }, true);

        public static ReplyKeyboardMarkup CancelKeyboard = new ReplyKeyboardMarkup(new[] { new KeyboardButton("Cancel") }, true);

        public static ReplyKeyboardMarkup SpecificKeyboard(Access access)
        {
            switch (access)
            {
                case Access.Manager: return Keyboard.StartKeyboard_Manager;
                case Access.Admin: return Keyboard.StartKeyboard_Admin;
                case Access.Member: return Keyboard.StartKeyboard_Member;
                default: return null;
            }
        }
    }
}
