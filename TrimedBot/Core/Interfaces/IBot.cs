﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Interfaces
{
    public interface IBot : ITelegramBotClient
    {
        //void SetProxy(Proxy proxy);
    }
}
