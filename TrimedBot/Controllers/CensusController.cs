using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;

namespace TrimedBot.Controllers
{
    [Route("census")]
    public class CensusController : ControllerBase
    {
        private IUser user;
        private DB db;

        public CensusController(IUser user, DB db)
        {
            this.user = user;
            this.db = db;
        }

        [Route("users")]
        public async Task<IActionResult> Users()
        {
            var users = await user.GetUsersAsync();
            StringBuilder sb = new();
            sb.AppendLine($"Number of users: {users.Count()}");
            foreach (var u in users)
            {
                sb.AppendLine(JsonConvert.SerializeObject(u));
            }
            return Content(sb.ToString());
        }
    }
}
