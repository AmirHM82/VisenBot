﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class Token
    {
        [Key]
        public Guid TokenCode { get; set; }
    }
}