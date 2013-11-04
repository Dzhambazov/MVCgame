using System;
using System.Collections.Generic;

namespace FirebirdWars.Models
{
    public class Unit
    {
        private ICollection<UnitRequirement> unitRequirements;

        public Unit()
        {
            this.unitRequirements = new HashSet<UnitRequirement>();
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public int WoodCost { get; set; }

        public int FoodCost { get; set; }
        
        public int IronCost { get; set; }

        public int PopulationCost { get; set; }

        public int Speed { get; set; }

        public int CarryingCapacity { get; set; }

        public int BuildTime { get; set; }

        public int InfantryPower { get; set; }

        public int CavalryPower { get; set; }

        public int SiegeArtilleryPower { get; set; }

        public int HealthPoints { get; set; }

        public virtual ICollection<UnitRequirement> UnitRequirements
        {
            get
            {
                return this.unitRequirements;
            }
            set
            {
                this.unitRequirements = value;
            }
        }
    }
}