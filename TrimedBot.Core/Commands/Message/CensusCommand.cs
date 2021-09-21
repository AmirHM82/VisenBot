using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Message
{
    public class CensusCommand : ICommand
    {
        private ObjectBox objectBox;

        public CensusCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            if (objectBox.User.Access == DAL.Enums.Access.Manager)
            {
                var userService = objectBox.Provider.GetRequiredService<IUser>();
                var users = await userService.GetUsersAsync();
                StringBuilder sb = new();
                sb.AppendLine($"Number of users: {users.Count()}");
                foreach (var u in users)
                {
                    sb.AppendLine(JsonConvert.SerializeObject(u));
                }
                new TextResponseProcessor()
                {
                    ReceiverId = objectBox.ChatId,
                    Text = sb.ToString()
                }.AddThisMessageToService(objectBox.Provider);
            }
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
