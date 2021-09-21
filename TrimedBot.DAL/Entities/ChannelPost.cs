using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class ChannelPost
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public Media Media { get; set; }
    }
}
