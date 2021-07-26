using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Classes.Processors.ProcessorTypes
{
    public class MultiProcessor : Processor
    {
        public List<Processor> messages;
        //public IServiceProvider provider;

        public MultiProcessor(List<Processor> messages/*, IServiceProvider provider*/)/* : base(provider)*/
        {
            this.messages = messages;
            //this.provider = provider;
        }

        protected async override Task Action(IServiceProvider provider)
        {
            List<Exception> exceptions = new();
            if (messages.Count > 0)
            {
                foreach (var item in messages)
                {
                    try
                    {
                        await item.Process(provider);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }

                if (exceptions.Count > 0) throw new AggregateException(exceptions);
            }
        }
    }
}
