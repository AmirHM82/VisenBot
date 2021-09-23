using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes.Processors
{
    public abstract class Processor
    {
        //public IServiceProvider Provider { get; set;  }

        protected Processor(/*IServiceProvider provider*/)
        {
            //Provider = provider;
        }

        public event ProcessorEventHandler OnFail;
        public event ProcessorEventHandler OnSuccess;

        public (sbyte counter, int errorCode) FailProp { get; set; }

        public bool Success { get; private set; }

        protected abstract Task Action(IServiceProvider provider);

        public async Task<bool> Process(IServiceProvider provider)
        {
            try
            {
                await Action(provider);
                Success = true;
                OnSuccess?.Invoke(this);
            }
            catch (ApiRequestException e)
            {
                FailProp = (FailProp.counter, e.ErrorCode);
                Success = false;
                OnFail?.Invoke(this);


                e.Message.LogError();
                if (e.InnerException is not null)
                    e.InnerException.Message.LogError();
                throw;
            }
            return Success;
        }

        public void AddThisMessageToService(IServiceProvider provider)
        {
            var responseService = provider.GetRequiredService<ResponseService>();
            responseService.AddMessage(this);
        }
    }

    public delegate void ProcessorEventHandler(Processor sender);
}
