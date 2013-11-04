using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FirebirdWars.Models
{
    public class UserBuilding
    {
        [Key]
        [Column("UserId", Order = 0)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Key]
        [Column("BuildingId", Order = 1)]
        [ForeignKey("Building")]
        public int BuildingId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Building Building { get; set; }

        [Required]
        public int BuildingLevel { get; set; }
    }
}