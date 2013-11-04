using FirebirdWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FirebirdWars.Models.Enums;
using FirebirdWars.Models.ViewModels;

namespace FirebirdWars.Controllers
{
    public class FarmController : BuildingsController
    {
        public ActionResult ShowProduction()
        {
            var context = new ApplicationDbContext();

            string userGuidId = User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);
            var building = user.Buildings.FirstOrDefault(ub => ub.Building.Type == "Farm");

            var currentWoodProduction = Calculate((int)BuildingsIncome.Wood, GetCoeficient(ResourceProductionCoeficient, building.BuildingLevel - 1));
            var nextLevelWoodProduction = Calculate((int)BuildingsIncome.Wood, GetCoeficient(ResourceProductionCoeficient, building.BuildingLevel));

            var buildingModel = new ResourceBuildingModel()
            {
                Type = building.Building.Type,
                CurrentProduction = currentWoodProduction,
                NextLevelProduction = nextLevelWoodProduction,
                ResourceType = "Food"
            };

            return PartialView("BuildingsPartials/_BuildingProductionPartial", buildingModel);
        }
    }
}