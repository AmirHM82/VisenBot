using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Context;

namespace TrimedBot.DAL
{
    public static class Migrate
    {
        public static void Entities(IServiceProvider provider)
        {
            var context = provider.CreateScope().ServiceProvider.GetRequiredService<DB>();

            if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
        }
    }
}
