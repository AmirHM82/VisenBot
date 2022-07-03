using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TrimedBot.DAL.Entities;
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
            new[] {new KeyboardButton("Search in posts") },
            //new[] {new KeyboardButton("Blocked tags") }
        }, true);

        public static ReplyKeyboardMarkup StartKeyboard_Admin() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Add new video") },
            new[] {new KeyboardButton("My videos") },
            new[] {new KeyboardButton("Posts") },
            new[] {new KeyboardButton("Search in posts") },
            //new[] {new KeyboardButton("Blocked tags") }
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
            new[] {new KeyboardButton("Send message to admins") },
            new[] {new KeyboardButton("Send message to all") },
            //new[] {new KeyboardButton("Blocked tags") },
            new[] {new KeyboardButton("Census") }
        }, true);

        public static ReplyKeyboardMarkup SettingsKeyboard() => new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new[] {new KeyboardButton("Tags") },
            new[] {new KeyboardButton("Channels") },
            new[] {new KeyboardButton("Ads properties")},
            new[] {new KeyboardButton("Token")},
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
                //InlineKeyboardButton.WithCallbackData("Edit title", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Title}/{MediaId}"),
                //InlineKeyboardButton.WithCallbackData("Edit caption", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Caption}/{MediaId}"),
                //InlineKeyboardButton.WithCallbackData("Edit video", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Video}/{MediaId}")
            
                InlineKeyboardButton.WithCallbackData("Properties", $"{CallbackSection.Post}/{CallbackSection.Properties}/{MediaId}")
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
            InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{mediaId}"),
            InlineKeyboardButton.WithCallbackData("Properties", $"{CallbackSection.Post}/{CallbackSection.Properties}/{mediaId}")
        });

        public static InlineKeyboardMarkup ConfirmedPublicMediaKeyboard(Guid mediaId) => new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Decline", $"{CallbackSection.Post}/{CallbackSection.Confirm}/{mediaId}"),
            InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Delete}/{mediaId}"),
            InlineKeyboardButton.WithCallbackData("Properties", $"{CallbackSection.Post}/{CallbackSection.Properties}/{mediaId}")
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

            InlineKeyboardButton[] t2 =
            {
                InlineKeyboardButton.WithCallbackData("Cancel", CallbackSection.Cancel)
            };

            return new InlineKeyboardMarkup(new[] { t1, t2 });
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

        public static InlineKeyboardMarkup ManageUser(Guid Id, long UserId, Access UserAccess, bool IsBanned)
        {
            InlineKeyboardButton[] t1 =
                    {
                    UserAccess == Access.Member ?
                    InlineKeyboardButton.WithCallbackData("Make admin", $"{CallbackSection.Admin}/{CallbackSection.Add}/{Id}") :
                    UserAccess == Access.Admin ?
                    InlineKeyboardButton.WithCallbackData("Delete admin", $"{CallbackSection.Admin}/{CallbackSection.Delete}/{Id}") :
                    InlineKeyboardButton.WithCallbackData("Delete manager", $"M/D/{Id}")
                    };

            InlineKeyboardButton[] t2 =
            {
                    InlineKeyboardButton.WithCallbackData("Send a message", $"{CallbackSection.User}/{CallbackSection.Send}/{CallbackSection.Message}/{UserId}")
                    };

            InlineKeyboardButton[] t3 =
            {
                    IsBanned ?
                    InlineKeyboardButton.WithCallbackData("Unban", $"{CallbackSection.User}/{CallbackSection.Unban}/{Id}")
                    : InlineKeyboardButton.WithCallbackData("Ban", $"{CallbackSection.User}/{CallbackSection.Ban}/{Id}")
                    };
            return new InlineKeyboardMarkup(new[] { t1, t2, t3 });
        }

        public static InlineKeyboardMarkup Tag(Tag tag)
        {
            InlineKeyboardButton[] result =
            {
                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Tag}/{CallbackSection.Delete}/{tag.Id}")
            };

            return new InlineKeyboardMarkup(result);
        }

        public static InlineKeyboardMarkup BlockedTag(Tag tag)
        {
            InlineKeyboardButton[] result =
            {
                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.User}/{CallbackSection.Tag}/{CallbackSection.Delete}/{tag.Id}")
            };

            return new InlineKeyboardMarkup(result);
        }

        public static InlineKeyboardMarkup PostsTag(Tag tag)
        {
            InlineKeyboardButton[] result =
            {
                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Post}/{CallbackSection.Tag}/{CallbackSection.Delete}/{tag.Id}")
            };

            return new InlineKeyboardMarkup(result);
        }

        public static InlineKeyboardMarkup AddTag()
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Add", $"{CallbackSection.Tag}/{CallbackSection.Add}")
            };

            return new InlineKeyboardMarkup(k1);
        }

        public static InlineKeyboardMarkup AddPostsTag(Guid postId)
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Add", $"{CallbackSection.Post}/{CallbackSection.Tag}/{CallbackSection.Add}/{postId}")
            };

            InlineKeyboardButton[] k2 =
            {
                InlineKeyboardButton.WithCallbackData("Cancel", CallbackSection.Cancel)
            };

            return new InlineKeyboardMarkup(new[] { k1, k2 });
        }

        public static InlineKeyboardMarkup AddBlockedTag()
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Add", $"{CallbackSection.User}/{CallbackSection.Tag}/{CallbackSection.Add}")
            };

            return new InlineKeyboardMarkup(k1);
        }

        public static InlineKeyboardMarkup PrivatePostProperties(Guid postId, bool HasCancel)
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Edit title", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Title}/{postId}"),
                InlineKeyboardButton.WithCallbackData("Edit caption", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Caption}/{postId}"),
                InlineKeyboardButton.WithCallbackData("Edit video", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Video}/{postId}")
            };

            if (HasCancel)
            {
                InlineKeyboardButton[] k2 =
                {
                    InlineKeyboardButton.WithCallbackData("Cancel", CallbackSection.Cancel)
                };

                return new InlineKeyboardMarkup(new[] { k1, k2 });
            }
            else
            {
                return new InlineKeyboardMarkup(new[] { k1 });
            }
        }

        public static InlineKeyboardMarkup PublicPostProperties(Guid postId, bool HasCancel)
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Edit title", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Title}/{postId}"),
                InlineKeyboardButton.WithCallbackData("Edit caption", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Caption}/{postId}"),
                InlineKeyboardButton.WithCallbackData("Edit video", $"{CallbackSection.Post}/{CallbackSection.Edit}/{CallbackSection.Video}/{postId}")
            };

            //InlineKeyboardButton[] k2 =
            //{
            //    InlineKeyboardButton.WithCallbackData("Edit tags", $"{CallbackSection.Post}/{CallbackSection.Tag}/{CallbackSection.Next}/1")
            //};

            if (HasCancel)
            {
                InlineKeyboardButton[] k3 =
                {
                    InlineKeyboardButton.WithCallbackData("Cancel", CallbackSection.Cancel)
                };

                return new InlineKeyboardMarkup(new[] { k1, /*k2,*/ k3 });
            }
            else
            {
                return new InlineKeyboardMarkup(new[] { k1, /*k2*/ });
            }
        }

        public static InlineKeyboardMarkup Channel(DAL.Entities.Channel channel)
        {
            InlineKeyboardButton[] result =
            {
                InlineKeyboardButton.WithCallbackData("Delete", $"{CallbackSection.Channel}/{CallbackSection.Delete}/{channel.Id}")
            };

            return new InlineKeyboardMarkup(result);
        }

        public static InlineKeyboardMarkup AddChannel()
        {
            InlineKeyboardButton[] k1 =
            {
                InlineKeyboardButton.WithCallbackData("Add", $"{CallbackSection.Channel}/{CallbackSection.Add}")
            };

            return new InlineKeyboardMarkup(k1);
        }

    }
}
