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
    public class TokenServices : IToken
    {
        protected DB _db;

        public TokenServices(DB db)
        {
            _db = db;
        }

        public async Task Add(Token token)
        {
            await _db.Tokens.AddAsync(token);
        }

        public async Task<Token> GetToken()
        {
            return await _db.Tokens.FirstOrDefaultAsync();
        }

        public void Remove(Token token)
        {
            _db.Tokens.Remove(token);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Verify(string Code)
        {
            Token token = await _db.Tokens.SingleOrDefaultAsync(x => x.Code.Equals(Code));
            //_db.Entry(token).State = EntityState.Deleted;
            if (token != null)
                return true;
            return false;
        }
    }
}
