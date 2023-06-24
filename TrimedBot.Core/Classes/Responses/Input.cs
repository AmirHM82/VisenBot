using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes.Responses.ResponseTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Responses
{
    public abstract class Input
    {
        private ObjectBox objectBox;

        public Input(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public bool Status { get; set; }

        public abstract Task Action(List<Func<Task>> cmds);

        public async Task Response()
        {
            try
            {
                List<Func<Task>> cmds = new List<Func<Task>>();

                await Action(cmds);

                foreach (var item in cmds)
                {
                    await item();
                }

                await objectBox.DeleteTemps(objectBox.ChatId);
                await objectBox.UpdateDatabaseUserInfo();
                await objectBox.UpdateDatabaseChannelInfo();
                Status = true;
            }
            catch (Exception e)
            {
                "Input:".LogError();
                e.TargetSite.Name.LogError();
                e.Message.LogError();
                if (e.InnerException is not null)
                    e.InnerException.Message.LogError();

                Status = false;
                throw;
            }
        }
    }
}
