using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Enums;

namespace TrimedBot.DAL.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public UserState UserState { get; set; }
        public UserState LastUserState { get; set; }
        //public ICollection<Media> Medias { get; set; }
        public Access Access { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsSentAdminRequest { get; set; }
        public DateTime? RequestDate { get; set; }
        public bool IsBanned { get; set; }
        public string Temp { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Banner> Banners { get; set; }
        public ICollection<Tag> BlockedTags { get; set; }
    }
}
