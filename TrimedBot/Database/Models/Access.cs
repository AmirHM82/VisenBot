using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Database.Models
{
    [Flags]
    public enum Access : byte
    {
        Manager = 0b000000,
        Admin = 0b000001,
        Member = 0b000010
    }
}
