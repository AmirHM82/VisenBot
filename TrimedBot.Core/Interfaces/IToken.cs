using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface IToken
    {
        Task Add(Token token);        
        Task<Token> GetToken();
        void Save();
        void Remove(Token token);
        Task<bool> Verify(Guid TokenCode);
    }
}
