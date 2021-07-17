using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class Media
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string FileId { get; set; }
        public virtual User User { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? AddDate { get; set; }
    }
}
