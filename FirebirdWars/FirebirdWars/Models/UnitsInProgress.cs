using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class UnitsInProgress
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Unit")]
        public int UnitId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Unit Unit { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime DateEnded { get; set; }

        public int Quantity { get; set; }
    }
}