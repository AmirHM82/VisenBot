using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrimedBot.ConsoleApp.Request;

namespace TrimedBot.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new WtfClass().SendUpdateTest();

            await new WtfClass().Do();
        }
    }
}
