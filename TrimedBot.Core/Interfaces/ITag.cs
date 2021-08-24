using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface ITag : IDisposable
    {
        Task AddAsync(Tag tag);
        void Delete(Tag tag);
        Task Delete(int tagId);
        Task<Tag> FindAsync(int tagId);
        Task<List<Tag>> Search(string name);
        Task SaveAsync();
        void Update(Tag tag);
        Task<List<Tag>> GetTagsAsync(int pageNum);
    }
}
