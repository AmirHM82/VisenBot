using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Database.Models
{
    public class TempMessage
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public long UserId { get; set; }
    }
}
