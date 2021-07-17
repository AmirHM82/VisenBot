using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Enums
{
    public enum KeyboardType : byte
    {
        NoOne = 0,
        StartKeyboard_Member = 1,
        StartKeyboard_Admin = 2,
        StartKeyboard_Manager = 3,
        SettingsKeyboard = 4,
        AdsPropertiesKeyboard = 5,
        CancelKeyboard = 6,
        PrivateMediaKeyboard = 7,
        NPKeyboard = 8
    }
}
