using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class BuildingsInProgress
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Building")]
        public int BuildingId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Building Building { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime DateEnded { get; set; }

        public int UpgradeLevel { get; set; }
    }
}