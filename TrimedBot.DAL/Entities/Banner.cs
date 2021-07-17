using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageFileId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ShowDate { get; set; }
        public User User { get; set; }
    }
}
