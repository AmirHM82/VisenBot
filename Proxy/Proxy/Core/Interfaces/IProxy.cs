using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Core.Interfaces
{
    public interface IProxy
    {
        Task AddAsync(DataBase.Model.Proxy proxy);
        void Delete(List<DataBase.Model.Proxy> proxies);
        Task<List<DataBase.Model.Proxy>> GetAsync(int num);
        Task SaveAsync();
    }
}
