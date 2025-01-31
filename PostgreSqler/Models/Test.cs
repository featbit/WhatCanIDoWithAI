using Microsoft.EntityFrameworkCore;
using PostgreSqler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgreSqler.Models
{
    public class Test
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("testf")]
        public string Testf { get; set; }
    }
}
