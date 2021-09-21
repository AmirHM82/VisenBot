using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Enums;

namespace TrimedBot.DAL.Entities
{
    public class TempMessage
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public long ChatId { get; set; }
        public TempType Type { get; set; }
    }
}
