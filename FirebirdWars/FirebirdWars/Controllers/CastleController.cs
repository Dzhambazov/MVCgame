using FirebirdWars.Models;
using FirebirdWars.Models.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FirebirdWars.Controllers
{
    public class CastleController : BuildingsController
    {
        public ActionResult GetAllAvailableBuildings()
        {
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var userBuildings = new List<UserBuilding>();
            var userBuildingsString = new List<string>();

            var userBuildingsInProgress = new List<BuildingsInProgress>();
            var userBuildingsInProgressString = new List<string>();

            if (user != null)
            {
                userBuildings = user.Buildings.ToList();
                userBuildingsString = userBuildings.Select(ub => ub.Building.Type).ToList();

                userBuildingsInProgress = user.BuildingsInProgress.ToList();
                userBuildingsInProgressString = userBuildingsInProgress.Select(ub => ub.Building.Type).ToList();
            }

            var result = context.Buildings.Where(b => !userBuildingsString.Contains(b.Type) && !userBuildingsInProgressString.Contains(b.Type)).ToList().Select(building => new BuildingModel()
            {
                Id = building.Id,
                Type = building.Type,
                WoodCost = building.WoodCost,
                FoodCost = building.FoodCost,
                IronCost = building.IronCost,
                PopulationCost = building.PopulationCost,
                BuildTime = SecondsToTime(building.BuildTime),
                IsConstructable = IsConstructable(building, user)
            });

            return PartialView("BuildingsPartials/_AllBuildingsPartialView", result);
        }

        public ActionResult GetAllUserBuildingsInProgress()
        {
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var result = user.BuildingsInProgress.ToList().Select(building => new UserBuildingsInProgressModel()
            {
                Id = building.Id,
                Type = building.Building.Type,
                CompletionDate = building.DateEnded
            });

            return PartialView("BuildingsPartials/_UserBuildingsInProgressPartial", result);
        }

        public ActionResult GetAllUserUnits()
        {
            var context = new ApplicationDbContext();
            string userId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(usr => usr.Id == userId);

            var result = user.Units.Where(u => u.UnitCount > 0).Select(unit => new UserUnitsVM()
            {
                 Unit = unit.Unit.Type,
                 Count = Convert.ToInt32(unit.UnitCount)
            });

            return PartialView("BuildingsPartials/_UserUnitsPV", result);
        }
 

        public ActionResult CreateBuilding(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var building = context.Buildings.FirstOrDefault(b => b.Id == id);

            if (building == null)
            {
                return HttpNotFound();
            }

            string userGuidId = User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);
            var currentBuildingExist = user.Buildings.FirstOrDefault(ub => ub.BuildingId == id);

            if (currentBuildingExist != null)
            {
                return Content("<script>alert('You already have that building! Please upgrade it.'); window.history.go(-1); </script>");
            }

            if (!IsConstructable(building, user))
            {
                return Content("<script>alert('You cant build this!'); window.history.go(-1);</script>");
            }

            user.Wood -= building.WoodCost;
            user.Food -= building.FoodCost;
            user.Iron -= building.IronCost;
            user.Population += building.PopulationCost;

            user.BuildingsInProgress.Add(new BuildingsInProgress()
            {
                Building = building,
                User = user,
                UpgradeLevel = 1,
                DateEnded = DateTime.Now.AddSeconds(building.BuildTime)
            });

            context.SaveChanges();

            return RedirectToAction("Building/1", "Buildings");
        }

        public ActionResult GetUserBuildings()
        {
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var userBuildings = user.Buildings.ToList();

            var result = userBuildings.Select(building => new UserBuildingModel()
            {
                Id = building.BuildingId,
                Type = building.Building.Type,
                Level = building.BuildingLevel,
                RequiredWood = this.Calculate(building.Building.WoodCost, this.GetCoeficient(ResourceCoeficient, building.BuildingLevel)),
                RequiredFood = this.Calculate(building.Building.FoodCost, this.GetCoeficient(ResourceCoeficient, building.BuildingLevel)),
                RequiredIron = this.Calculate(building.Building.IronCost, this.GetCoeficient(ResourceCoeficient, building.BuildingLevel)),
                RequiredPopulation = building.Building.PopulationCost,
                BuildTime = this.SecondsToTime(this.Calculate(building.Building.BuildTime, this.GetCoeficient(BuildTimeCoeficient, building.BuildingLevel)))
            });

            return PartialView("BuildingsPartials/_UserConstructedBuildingsPartial", result);
        }

        public ActionResult UpgradeBuilding(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var building = context.Buildings.FirstOrDefault(b => b.Id == id);

            if (building == null)
            {
                return HttpNotFound();
            }

            string userGuidId = User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);
            var currentBuilding = user.Buildings.FirstOrDefault(ub => ub.BuildingId == id);

            if (currentBuilding == null)
            {
                return Content("<script>alert('You can't upgrade building that you don't have !'); window.history.go(-1);</script>");
            }

            var coeficient = this.GetCoeficient(ResourceCoeficient, currentBuilding.BuildingLevel);
            var woodCost = this.Calculate(currentBuilding.Building.WoodCost, coeficient);
            var foodCost = this.Calculate(currentBuilding.Building.FoodCost, coeficient);
            var ironCost = this.Calculate(currentBuilding.Building.IronCost, coeficient);

            if (user.Wood - woodCost < 0 || user.Food - foodCost < 0 || user.Iron - ironCost < 0)
            {
                return Content("<script>alert('You dont have enough resources!'); window.history.go(-1);</script>");
            }

            var userBuildingsInProgressCount = user.BuildingsInProgress.Count;

            if (userBuildingsInProgressCount >= MaxBuildingsInProgress)
            {
                return Content("<script>alert('You cant build this!'); window.history.go(-1);</script>");
            }

            var buildTimeCoeficient = this.GetCoeficient(BuildTimeCoeficient, currentBuilding.BuildingLevel);
            var buildTime = this.Calculate(currentBuilding.Building.BuildTime, buildTimeCoeficient);

            user.Wood -= woodCost;
            user.Food -= foodCost;
            user.Iron -= ironCost;
            user.Population += currentBuilding.Building.PopulationCost;

            user.BuildingsInProgress.Add(new BuildingsInProgress()
            {
                Building = currentBuilding.Building,
                User = user,
                UpgradeLevel = currentBuilding.BuildingLevel + 1,
                DateEnded = DateTime.Now.AddSeconds(buildTime)
            });

            context.SaveChanges();

            return RedirectToAction("Building/1", "Buildings");
        }

        public ActionResult CancelCreateBuilding(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var buildingInProgress = context.BuildingsInProgress.FirstOrDefault(bip => bip.Id == id);

            if (buildingInProgress == null)
            {
                return HttpNotFound();
            }

            string userGuidId = User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            user.BuildingsInProgress.Remove(buildingInProgress);
            context.BuildingsInProgress.Remove(buildingInProgress);

            context.SaveChanges();

            return RedirectToAction("Building/1", "Buildings");
        }

        private bool IsConstructable(Building building, ApplicationUser user)
        {
            var context = new ApplicationDbContext();

            //var buildingRequirements = context.BuildingRequirements.Where(br => br.BuildingId == buildingId);

            bool isConstructable = false;

            var userBuildingsInProgressCount = user.BuildingsInProgress.Count;

            if (user.Wood >= building.WoodCost &&
                user.Food >= building.FoodCost &&
                user.Iron >= building.IronCost &&
                user.MaxPopulation > building.PopulationCost + user.Population &&
                userBuildingsInProgressCount < MaxBuildingsInProgress)
            {
                isConstructable = true;
            }

            return isConstructable;
        }

        public JsonResult GetRequirements([DataSourceRequest]DataSourceRequest request, int? id)
        {
            //TODO : needs to be fixed
            var context = new ApplicationDbContext();

            var result = context.BuildingRequirements.Where(br => br.BuildingId == id).ToList().Select(building => new BuildingRequirementsModel()
            {
                Id = building.Building.Id,
                BuildingType = building.RequiredBuilding.Type,
                BuildingLevel = building.RequiredBuildingLevel
            });

            if (result.Count() == 0)
            {
                result = new List<BuildingRequirementsModel>() 
                {
                    new BuildingRequirementsModel() 
                    {
                        Id = 0,
                        BuildingType = "[ None ]",
                        BuildingLevel = 0
                    }
                };
            }

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
	}
}