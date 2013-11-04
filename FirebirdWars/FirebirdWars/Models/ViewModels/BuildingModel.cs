
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class BuildingModel
    {
        //public static Expression<Func<Building, BuildingModel>> FromBuilding
        //{
        //    get
        //    {
        //        return building => new BuildingModel()
        //        {
        //            Id = building.Id,
        //            Type = building.Type,
        //            WoodCost = building.WoodCost,
        //            FoodCost = building.FoodCost,
        //            IronCost = building.IronCost,
        //            PopulationCost = building.PopulationCost,
        //            BuildTime = SecondsToTime(building.BuildTime)
        //        };
        //    }
        //}
        
        public int Id { get; set; }

        public string Type { get; set; }

        public int WoodCost { get; set; }

        public int FoodCost { get; set; }

        public int IronCost { get; set; }

        public int PopulationCost { get; set; }

        public string BuildTime { get; set; }

        public bool IsConstructable { get; set; }
    }
}