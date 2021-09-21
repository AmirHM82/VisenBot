using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Classes.Processors.ProcessorTypes;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.Core.Classes.Processors;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace TrimedBot.Core.Services
{
    public class ResponseService
    {
        private IServiceProvider provider;

        public ResponseService(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public bool State { get; set; }
        public ConcurrentQueue<Processor> Messages { get; } = new ConcurrentQueue<Processor>();
        public ConcurrentQueue<Processor> Faileds { get; } = new ConcurrentQueue<Processor>();

        public async Task StartProccesingMessages()
        {
            State = true;

            while (State)
            {
                var selecteds = SelectMessages();
                if (selecteds.Count > 0)
                    foreach (var item in selecteds)
                    {
                        try
                        {
                            Task.WhenAll(item.Process(provider), Task.Delay(100));
                            //await Task.Delay(300);
                        }
                        catch
                        {
                            if (item.FailProp.counter <= 3) AddFailed(item);
                        }
                    }
                else await Task.Delay(30);
            }
        }

        private List<Processor> SelectMessages()
        {
            (int messagesCount, int failedsCount) counts = (Messages.Count(), Faileds.Count());
            (int messages, int faileds) nums = (0, 0);

            if (counts.messagesCount > 0 && counts.failedsCount == 0)
            {
                nums = (14, 0);
            }
            else if (counts.messagesCount == 0 && counts.failedsCount > 0)
            {
                nums = (0, 14);
            }
            else if (counts.messagesCount > 0 && counts.failedsCount > 0)
            {
                nums = (7, 7);
            }

            return PickEachList(nums.messages, nums.faileds);
        }

        private List<Processor> PickEachList(int message, int failed)
        {
            List<Processor> cs = new();
            while (failed > 0)
            {
                if (Faileds.TryDequeue(out Processor c))
                {
                    cs.Add(c);
                }
                else break;
                failed--;
            }
            while (message > 0)
            {
                if (Messages.TryDequeue(out Processor c))
                {
                    BindEvents(c);
                    cs.Add(c);
                }
                else break;
                message--;
            }
            return cs;
        }

        private void BindEvents(Processor c)
        {
            c.OnSuccess += OnSuccess;
            c.OnFail += OnFail;
        }

        private void OnSuccess(Processor sender)
        {
        }

        private void OnFail(Processor sender)
        {
            var counter = sender.FailProp.counter;
            sender.FailProp = (++counter, sender.FailProp.errorCode);
        }

        private void UnBindEvents(Processor c)
        {
            c.OnSuccess -= OnSuccess;
            c.OnFail -= OnFail;
        }

        public void StopResponsing()
        {
            State = false;
        }

        public void AddMessage(Processor processor)
        {
            if (processor is not null) Messages.Enqueue(processor);
        }

        public void AddFailed(Processor processor)
        {
            if (processor is not null) Faileds.Enqueue(processor);
        }

        public bool IsExist(Processor processor)
        {
            bool result = false;
            if (Messages.Equals(processor)) result = true;
            return result;
        }
    }
}
