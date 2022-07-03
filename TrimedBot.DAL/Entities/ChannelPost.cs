using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class ChannelPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MessageId { get; set; }
        public Media Media { get; set; }
    }
}
