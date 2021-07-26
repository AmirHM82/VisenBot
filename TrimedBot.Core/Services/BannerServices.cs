using System;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TrimedBot.Core.Services
{
    public class BannerServices : IBanner
    {
        private DB context;

        public BannerServices(DB context)
        {
            this.context = context;
        }

        public async Task AddAsync(Banner banner)
        {
            await context.Banners.AddAsync(banner);
        }

        public void Delete(Banner banner)
        {
            context.Banners.Remove(banner);
        }

        public async Task<Banner> FindAsync(Guid id)
        {
            return await context.Banners.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Banner>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}