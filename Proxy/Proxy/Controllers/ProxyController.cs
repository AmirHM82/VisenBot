using Microsoft.AspNetCore.Mvc;
using Proxy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Controllers
{
    public class ProxyController : Controller
    {
        protected IProxy _proxy;

        public ProxyController(IProxy proxy)
        {
            _proxy = proxy;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add(DataBase.Model.Proxy proxy)
        {
            await _proxy.AddAsync(proxy);
            return View();
        }
        
        [HttpGet]
        public async Task<List<DataBase.Model.Proxy>> Get(int num)
        {
            var proxies = await _proxy.GetAsync(num);
            return proxies;
        }
    }
}
