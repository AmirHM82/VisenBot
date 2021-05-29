using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Database.Context;
using TrimedBot.Database.Models;

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
            Token token = await _db.Tokens.SingleOrDefaultAsync();
            return token;
        }

        public void Remove(Token token)
        {
            _db.Tokens.Remove(token);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<bool> Verify(Guid TokenCode)
        {
            Token token = await _db.Tokens.SingleOrDefaultAsync(x => x.TokenCode.Equals(TokenCode));
            //_db.Entry(token).State = EntityState.Deleted;
            if (token != null)
                return true;
            return false;
        }
    }
}
