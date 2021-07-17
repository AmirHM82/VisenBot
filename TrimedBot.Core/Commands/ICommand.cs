using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Core.Commands
{
    public interface ICommand
    {
        Task Do();
        Task UnDo();
    }
}
