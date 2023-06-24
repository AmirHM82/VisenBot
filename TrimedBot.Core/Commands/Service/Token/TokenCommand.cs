using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Commands.Service.Token
{
    public class TokenCommand : ICommand
    {
        private ObjectBox objectBox;

        public TokenCommand(ObjectBox objectBox)
        {
            this.objectBox = objectBox;
        }

        public async Task Do()
        {
            bool IsNeedCreate = false;

            var tokenService = objectBox.Provider.GetRequiredService<IToken>();
            var token = await tokenService.GetToken();

            if (token is not null)
            {
                if (token.ExpireDate < DateTime.Now)
                {
                    tokenService.Remove(token);
                    IsNeedCreate = true;
                }
            }
            else IsNeedCreate = true;

            if (IsNeedCreate)
            {
                token = new DAL.Entities.Token
                {
                    Code = Guid.NewGuid().ToString(),
                    ExpireDate = DateTime.Now.AddMinutes(5)
                };

                await tokenService.Add(token);
                await tokenService.SaveAsync();
            }

            new TextResponseProcessor(objectBox)
            {
                ReceiverId = objectBox.User.UserId,
                Text = $"Code: {token.Code}\nExpire date: {token.ExpireDate}"
            }.AddThisMessageToService(objectBox.Provider);
        }

        public Task UnDo()
        {
            return Task.CompletedTask;
        }
    }
}
