using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Enums
{
    public enum MessageType : byte
    {
        Nothing = 0,
        Text = 1,
        Video = 2
    }
}
