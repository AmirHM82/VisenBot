using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Controllers
{
    [ApiController]
    [Route("api/responseservice")]
    public class ResponseServiceController : ControllerBase
    {
        protected ResponseService response;

        public ResponseServiceController(ResponseService response)
        {
            this.response = response;
        }

        public void StartResponsing()
        {
            response.StartProccesingMessages();
        }

        public void StopResponsing()
        {
            response.StopResponsing();
        }
    }
}
