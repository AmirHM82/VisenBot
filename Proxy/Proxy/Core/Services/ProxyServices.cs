using Microsoft.EntityFrameworkCore;
using Proxy.Core.Interfaces;
using Proxy.DataBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Core.Services
{
    public class ProxyServices : IProxy
    {
        protected DB _db;

        public ProxyServices(DB db)
        {
            _db = db;
        }

        public async Task AddAsync(DataBase.Model.Proxy proxy)
        {
            await _db.Proxies.AddAsync(proxy);
        }

        public void Delete(List<DataBase.Model.Proxy> proxies)
        {
            _db.Proxies.RemoveRange(proxies);
        }

        public async Task<List<DataBase.Model.Proxy>> GetAsync(int num)
        {
            return await _db.Proxies.Take(num).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
