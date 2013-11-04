using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class Building
    {
        private ICollection<BuildingRequirement> buildingRequirements;

        public Building()
        {
            this.buildingRequirements = new HashSet<BuildingRequirement>();
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public int WoodCost { get; set; }

        public int FoodCost { get; set; }

        public int IronCost { get; set; }

        public int PopulationCost { get; set; }

        public int BuildTime { get; set; }

        public string Coords { get; set; }

        public virtual ICollection<BuildingRequirement> BuildingRequirements
        {
            get
            {
                return this.buildingRequirements;
            }
            set
            {
                this.buildingRequirements = value;
            }
        }
    }
}