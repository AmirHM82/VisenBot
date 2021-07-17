using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class TempMessageServices : ITempMessage
    {
        protected DB _db;

        public TempMessageServices(DB db)
        {
            _db = db;
        }

        public Task AddAsync(List<TempMessage> tempMessages)
        {
            //return Task.Run(async () =>
            //{
            //    if (tempMessages != null)
            //        await _db.TempMessages.AddRangeAsync(tempMessages);
            //});

            if (tempMessages != null)
                return _db.TempMessages.AddRangeAsync(tempMessages);
            return null;

            //foreach (var item in tempMessages)
            //{
            //    await _db.TempMessages.AddAsync(item);
            //    //_db.Entry(item.User).State = EntityState.Detached;
            //    _db.Entry(item.User).State = EntityState.Unchanged;
            //}
        }

        public void Delete(List<TempMessage> tempMessages)
        {
            _db.TempMessages.RemoveRange(tempMessages);
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

        public async Task<List<TempMessage>> GetAndDeleteAsync(long userId)
        {

            var messages = await _db.TempMessages.Where(x => x.UserId == userId).ToListAsync();
            foreach (var msg in messages)
            {
                _db.Entry(msg).State = EntityState.Deleted;
            }
            return messages;

            //return _db.TempMessages.Where(x => x.UserId == userId).ToListAsync();
        }

        public Task<TempMessage> FindAsync(long userId, int messageId)
        {
            //return Task.Run(async () =>
            //{
            //    var message = await _db.TempMessages.FirstOrDefaultAsync(x => x.UserId == userId && x.MessageId == messageId);
            //    return message;
            //});

            return _db.TempMessages.FirstOrDefaultAsync(x => x.UserId == userId && x.MessageId == messageId);
        }

        public void Delete(TempMessage tempMessage)
        {
            _db.TempMessages.Remove(tempMessage);
        }

        public Task Delete(long userId, int messageId)
        {
            return Task.Run(async () =>
            {
                var tempMessage = await FindAsync(userId, messageId);
                Delete(tempMessage);
            });
        }

        public Task AddAsync(TempMessage tempMessages)
        {
            return Task.Run(() =>
            {
                if (tempMessages != null)
                    _db.TempMessages.AddAsync(tempMessages);
            });
        }
    }
}
