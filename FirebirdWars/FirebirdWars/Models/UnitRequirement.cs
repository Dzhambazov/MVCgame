using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class UnitRequirement
    {
        [Key]
        [Column("UnitId", Order = 0)]
        [ForeignKey("Unit")]
        public int UnitId { get; set; }

        [Key]
        [Column("RequiredBuildingId", Order = 1)]
        [ForeignKey("RequiredBuilding")]
        public int RequiredBuildingId { get; set; }

        public virtual Unit Unit { get; set; }

        public virtual Building RequiredBuilding { get; set; }

        [Required]
        public int RequiredBuildingLevel { get; set; }
    }
}