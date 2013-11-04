using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FirebirdWars.Models
{
    public class UserUnit
    {
        [Key]
        [Column("UserId", Order = 0)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Key]
        [Column("UnitId", Order = 1)]
        [ForeignKey("Unit")]
        public int UnitId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Unit Unit { get; set; }

        [Required]
        public int UnitCount { get; set; }
    }
}