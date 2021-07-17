using System;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Sections;

namespace TrimedBot.Core.Classes
{
    public static class Keyboard
    {
        public static ReplyKeyboardMarkup StartKeyboard_Member() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Send admin request") },
            new[] {new KeyboardButton("Search in posts") }
        }, true);

        public static ReplyKeyboardMarkup StartKeyboard_Admin() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Posts") },
            new[] {new KeyboardButton("Search in posts") }
        }, true);

        public static ReplyKeyboardMarkup StartKeyboard_Manager() => new ReplyKeyboardMarkup(new KeyboardButton[][]
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

        public static ReplyKeyboardMarkup SettingsKeyboard() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Ads properties")},
            new[] { new KeyboardButton("Cancel") }
        }, true);

        public static ReplyKeyboardMarkup AdsPropertiesKeyboard() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("PerMemberAdsPrice") },
            new[] {new KeyboardButton("BasicAdsPrice") },
            new[] {new KeyboardButton("NumberOfAdsPerDay") },
            new[] { new KeyboardButton("Cancel") }
        }, true);

        public static ReplyKeyboardMarkup CancelKeyboard() => new ReplyKeyboardMarkup(new[] { new KeyboardButton("Cancel") }, true);

        public static ReplyKeyboardMarkup GetSpecificKeyboard(Access access)
        {
            switch (access)
            {
                case Access.Manager: return Keyboard.StartKeyboard_Manager();
                case Access.Admin: return Keyboard.StartKeyboard_Admin();
                case Access.Member: return Keyboard.StartKeyboard_Member();
                default: return null;
            }
        }

        public static ReplyKeyboardMarkup GetSpecificKeyboard(KeyboardType type)
        {
            switch (type)
            {
                case KeyboardType.StartKeyboard_Member: return StartKeyboard_Member();
                case KeyboardType.StartKeyboard_Admin: return StartKeyboard_Admin();
                case KeyboardType.StartKeyboard_Manager: return StartKeyboard_Manager();
                case KeyboardType.SettingsKeyboard: return SettingsKeyboard();
                case KeyboardType.AdsPropertiesKeyboard: return AdsPropertiesKeyboard();
                case KeyboardType.CancelKeyboard: return CancelKeyboard();
                default: return null;
            }
        }

        public static KeyboardType GetSpecificKeyboardType(Access access)
        {
            switch (access)
            {
                case Access.Manager: return KeyboardType.StartKeyboard_Manager;
                case Access.Admin: return KeyboardType.StartKeyboard_Admin;
                case Access.Member: return KeyboardType.StartKeyboard_Member;
                default: return KeyboardType.NoOne;
            }
        }

        public static InlineKeyboardMarkup PrivateMediaKeyboard(Guid MediaId)
        {
            InlineKeyboardButton[] t1 =
            {
                InlineKeyboardButton.WithCallbackData("Edit title", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Title}/{MediaId}"),
                InlineKeyboardButton.WithCallbackData("Edit caption", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Caption}/{MediaId}"),
                InlineKeyboardButton.WithCallbackData("Edit video", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Video}/{MediaId}")
            };

            InlineKeyboardButton[] t2 =
            {
                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{MediaId}")
            };

            return new InlineKeyboardMarkup(new[] { t1, t2 });
        }

        public static InlineKeyboardMarkup DeclinedPublicMediaKeyboard(Guid mediaId) => new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Confirm", $"{CallbackSection.Post}/{CallbackSection.Confirm}/{mediaId}"),
            InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{mediaId}")
        });

        public static InlineKeyboardMarkup ConfirmedPublicMediaKeyboard(Guid mediaId) => new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Decline", $"{CallbackSection.Post}/{CallbackSection.Confirm}/{mediaId}"),
            InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{mediaId}")
        });

        public static InlineKeyboardMarkup NPKeyboard(int pageNumber, string category)
        {
            int nextPage = pageNumber + 1;
            int previousPage = pageNumber - 1;

            InlineKeyboardButton[] t1 =
            {
                InlineKeyboardButton.WithCallbackData("Next", $"{category}/{CallbackSection.Next}/{nextPage}"),
                InlineKeyboardButton.WithCallbackData("Previous", $"{category}/{CallbackSection.Previous}/{previousPage}")
            };

            return new InlineKeyboardMarkup(new[] { t1 });
        }

        public static InlineKeyboardMarkup AdminRequest(long userId) => new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Accept", $"{CallbackSection.Admin}/{CallbackSection.Request}/{CallbackSection.Accept}/{userId}"),
            InlineKeyboardButton.WithCallbackData("Refuse", $"{CallbackSection.Admin}/{CallbackSection.Request}/{CallbackSection.Refuse}/{userId}"),
        });

        public static InlineKeyboardMarkup AdminDelete(Guid adminId) => new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Admin}/{CallbackSection.Delete}/{adminId}")
        });

        public static InlineKeyboardMarkup ManageUser(Guid UserId, Access UserAccess, bool IsBanned)
        {
            InlineKeyboardButton[] t1 =
                    {
                    UserAccess == Access.Member ?
                    InlineKeyboardButton.WithCallbackData("Make admin", $"{CallbackSection.Admin}/{CallbackSection.Add}/{UserId}") :
                    UserAccess == Access.Admin ?
                    InlineKeyboardButton.WithCallbackData("Delete admin", $"{CallbackSection.Admin}/{CallbackSection.Delete}/{UserId}") :
                    InlineKeyboardButton.WithCallbackData("Delete manager", $"M/D/{UserId}")
                    };

            InlineKeyboardButton[] t2 =
            {
                    InlineKeyboardButton.WithCallbackData("Send a message", $"{CallbackSection.User}/{CallbackSection.Send}/{CallbackSection.Message}/{UserId}")
                    };

            InlineKeyboardButton[] t3 =
            {
                    IsBanned ?
                    InlineKeyboardButton.WithCallbackData("Unban", $"{CallbackSection.User}/{CallbackSection.Unban}/{UserId}")
                    : InlineKeyboardButton.WithCallbackData("Ban", $"{CallbackSection.User}/{CallbackSection.Ban}/{UserId}")
                    };
            return new InlineKeyboardMarkup(new[] { t1, t2, t3 });
        }
    }
}
