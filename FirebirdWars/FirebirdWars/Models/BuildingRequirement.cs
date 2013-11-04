using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class BuildingRequirement
    {
        [Key]
        [Column("BuildingId", Order = 0)]
        [ForeignKey("Building")]
        public int BuildingId { get; set; }

        [Key]
        [Column("RequiredBuildingId", Order = 1)]
        [ForeignKey("RequiredBuilding")]
        public int RequiredBuildingId { get; set; }

        public virtual Building Building { get; set; }

        public virtual Building RequiredBuilding { get; set; }

        [Required]
        public int RequiredBuildingLevel { get; set; }
    }
}