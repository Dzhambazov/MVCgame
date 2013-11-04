using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class AvailableUnitsModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int WoodCost { get; set; }

        public int FoodCost { get; set; }

        public int IronCost { get; set; }

        public int PopulationCost { get; set; }

        public int Speed { get; set; }

        public int CarryingCapacity { get; set; }

        public string BuildTime { get; set; }

        public int InfantryPower { get; set; }

        public int CavalryPower { get; set; }

        public int SiegeArtilleryPower { get; set; }

        public int HealthPoints { get; set; }

        public int MaxUnits { get; set; }
    }
}