using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Enums
{
    public enum ChannelType : byte
    {
        NoOne = 0,
        Admins = 1,
        Public = 2,
        Backup = 3
    }
}
