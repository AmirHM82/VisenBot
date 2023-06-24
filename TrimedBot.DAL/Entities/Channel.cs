using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Enums;

namespace TrimedBot.DAL.Entities
{
    public class Channel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public bool IsVerified { get; set; }
        public ChannelState State { get; set; }
        public ChannelType Type { get; set; }
        //public List<ChannelPost> ChannelPosts { get; set; }
    }
}
