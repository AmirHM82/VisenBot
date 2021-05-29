using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Database.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public UserPlace UserPlace { get; set; }
        public ICollection<Media> Medias { get; set; }
        public Access Access { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsSentAdminRequest { get; set; }
        public DateTime? RequestDate { get; set; }
        public bool IsBanned { get; set; }
        public string Temp { get; set; }
        public string UserName { get; set; }
        public ICollection<Banner> Banners { get; set; }
    }
}
