using FirebirdWars.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Kendo.Mvc.Extensions;
using FirebirdWars.Areas.Administration.Models.ViewModels;
using FirebirdWars.Models.ViewModels;
using FirebirdWars.Controllers;

namespace FirebirdWars.Areas.Administration.Controllers
{
    public class AdministrationController : AdminRestrictController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Buildings
        public JsonResult GetAllBuildings([DataSourceRequest]DataSourceRequest request)
        {
            var context = new ApplicationDbContext();

            var result = context.Buildings.Where(b => b.Type != null).ToList().Select(building => new BuildingModelAdmin()
            {
                Id = building.Id,
                Type = building.Type,
                WoodCost = building.WoodCost,
                FoodCost = building.FoodCost,
                IronCost = building.IronCost,
                PopulationCost = building.PopulationCost,
                BuildTime = SecondsToTime(building.BuildTime),
            });

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CreateBuilding([DataSourceRequest] DataSourceRequest request, BuildingModelAdmin building)
        //{
        //    if (building != null && ModelState.IsValid)
        //    {
        //        var context = new ApplicationDbContext();
        //        context.Buildings.Add(new Building
        //        {
        //            Type = building.Type,
        //            WoodCost = building.WoodCost,
        //            IronCost = building.IronCost,
        //            FoodCost = building.FoodCost,
        //            PopulationCost = building.PopulationCost,
        //            BuildTime = Convert.ToInt32(building.BuildTime)
        //        });
        //        context.SaveChanges();
        //    }

           

        //    building.BuildTime = SecondsToTime(Convert.ToInt32(building.BuildTime));
        //    return Json(new[] { building }.ToDataSourceResult(request, ModelState));
        //}


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditBuilding([DataSourceRequest] DataSourceRequest request, BuildingModelAdmin building)
        {
            if (building != null && ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                var buildingToEdit = context.Buildings.FirstOrDefault(p => p.Id == building.Id);
                if (buildingToEdit != null)
                {
                    buildingToEdit.Type = building.Type;
                    buildingToEdit.WoodCost = building.WoodCost;
                    buildingToEdit.IronCost = building.IronCost;
                    buildingToEdit.FoodCost = building.FoodCost;
                    buildingToEdit.PopulationCost = building.PopulationCost;
                    buildingToEdit.BuildTime = TimeToSeconds(building.BuildTime);
                    context.SaveChanges();
                }
            }
            return Json(new[] { building }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBuilding([DataSourceRequest] DataSourceRequest request, BuildingModelAdmin building)
        {
            if (building != null)
            {
                if (building.Type == "Lumberjacks")
                {
                    return Json(new[] { new BuildingModelAdmin() }.ToDataSourceResult(request, ModelState));
                }
                var context = new ApplicationDbContext();
                var buildingToRemove = context.Buildings.FirstOrDefault(b => b.Id == building.Id);
                if (buildingToRemove != null)
                {
                    var buildings = context.UsersBuildings.Where(b => b.BuildingId == building.Id);

                    foreach (var b in buildings)
                    {
                        context.UsersBuildings.Remove(b);
                    }

                    var requiredBuildings = context.BuildingRequirements.Where(b => b.RequiredBuildingId == building.Id);

                    foreach (var b in requiredBuildings)
                    {
                        context.BuildingRequirements.Remove(b);
                    }

                    var requiredUnits = context.UnitRequirements.Where(b => b.RequiredBuildingId == building.Id);

                    foreach (var b in requiredUnits)
                    {
                        context.UnitRequirements.Remove(b);
                    }

                    context.Buildings.Remove(buildingToRemove);
                    context.SaveChanges();
                }
            }

            return Json(new[] { building }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Units
        public JsonResult GetAllUnits([DataSourceRequest]DataSourceRequest request)
        {
            var context = new ApplicationDbContext();

            var result = context.Units.Where(u => u.Type != null).ToList().Select(unit => new UnitModelAdmin()
                {
                    Id = unit.Id,
                    Type = unit.Type,
                    WoodCost = unit.WoodCost,
                    IronCost = unit.IronCost,
                    FoodCost = unit.FoodCost,
                    PopulationCost = unit.PopulationCost,
                    HealthPoints = unit.HealthPoints,
                    Speed = unit.Speed,
                    CarryingCapacity = unit.CarryingCapacity,
                    BuildTime = SecondsToTime(unit.BuildTime),
                    CavalryPower = unit.CavalryPower,
                    InfantryPower = unit.InfantryPower,
                    SiegeArtilleryPower = unit.SiegeArtilleryPower

                });

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CreateNewUnit([DataSourceRequest] DataSourceRequest request, UnitModelAdmin unit)
        //{
        //    if (unit != null && ModelState.IsValid)
        //    {
        //        var context = new ApplicationDbContext();
        //        context.Units.Add(new Unit
        //        {
        //            Type = unit.Type,
        //            WoodCost = unit.WoodCost,
        //            IronCost = unit.IronCost,
        //            FoodCost = unit.FoodCost,
        //            PopulationCost = unit.PopulationCost,
        //            BuildTime = Convert.ToInt32(unit.BuildTime),
        //            CarryingCapacity = unit.CarryingCapacity,
        //            Speed = unit.Speed,
        //            CavalryPower = unit.CavalryPower,
        //            SiegeArtilleryPower = unit.SiegeArtilleryPower,
        //            InfantryPower = unit.InfantryPower,
        //            HealthPoints = unit.HealthPoints,
        //        });


        //        context.SaveChanges();
        //    }
        //    unit.BuildTime = SecondsToTime(Convert.ToInt32(unit.BuildTime));
        //    return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        //}



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNewUnit([DataSourceRequest] DataSourceRequest request, UnitModelAdmin unit)
        {
            if (unit != null && ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                var newUnit = new Unit
                {
                    Type = unit.Type,
                    WoodCost = unit.WoodCost,
                    IronCost = unit.IronCost,
                    FoodCost = unit.FoodCost,
                    PopulationCost = unit.PopulationCost,
                    BuildTime = Convert.ToInt32(unit.BuildTime),
                    CarryingCapacity = unit.CarryingCapacity,
                    Speed = unit.Speed,
                    CavalryPower = unit.CavalryPower,
                    SiegeArtilleryPower = unit.SiegeArtilleryPower,
                    InfantryPower = unit.InfantryPower,
                    HealthPoints = unit.HealthPoints,
                };
                context.Units.Add(newUnit);

                context.UnitRequirements.Add(new UnitRequirement
                {
                    UnitId = newUnit.Id,
                    RequiredBuildingId = context.Buildings.FirstOrDefault(x => x.Type == "Barracks").Id,
                    RequiredBuildingLevel = 1
                });


                context.SaveChanges();
            }
            unit.BuildTime = SecondsToTime(Convert.ToInt32(unit.BuildTime));
            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }















        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUnit([DataSourceRequest] DataSourceRequest request, UnitModelAdmin unit)
        {
            if (unit != null && ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                var unitToEdit = context.Units.FirstOrDefault(p => p.Id == unit.Id);
                if (unitToEdit != null)
                {
                    unitToEdit.Type = unit.Type;
                    unitToEdit.WoodCost = unit.WoodCost;
                    unitToEdit.IronCost = unit.IronCost;
                    unitToEdit.FoodCost = unit.FoodCost;
                    unitToEdit.PopulationCost = unit.PopulationCost;
                    unitToEdit.BuildTime = TimeToSeconds(unit.BuildTime);
                    unitToEdit.Speed = unit.Speed;
                    unitToEdit.HealthPoints = unit.HealthPoints;
                    unitToEdit.CarryingCapacity = unit.CarryingCapacity;
                    unitToEdit.CavalryPower = unit.CavalryPower;
                    unitToEdit.InfantryPower = unit.InfantryPower;
                    unitToEdit.SiegeArtilleryPower = unit.SiegeArtilleryPower;

                    context.SaveChanges();
                }
            }

            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUnit([DataSourceRequest] DataSourceRequest request, UnitModelAdmin unit)
        {
            if (unit != null)
            {
                var context = new ApplicationDbContext();

                var removeFromUnits = context.Units.Where(u => u.Id == unit.Id);
                foreach (var u in removeFromUnits)
                {
                    context.Units.Remove(u);
                }

                var removeFromUnitsInProgress = context.UnitsInProgress.Where(u => u.Id == unit.Id);
                foreach (var u in removeFromUnitsInProgress)
                {
                    context.UnitsInProgress.Remove(u);
                }

                var removeFromUserUnits = context.UsersUnits.Where(u => u.UnitId == unit.Id);
                foreach (var u in removeFromUserUnits)
                {
                    context.UsersUnits.Remove(u);
                }

                var buildingToRemove = context.Units.FirstOrDefault(b => b.Id == unit.Id);
                if (buildingToRemove != null)
                {
                    context.Units.Remove(buildingToRemove);
                    context.SaveChanges();
                }
            }

            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Users

        public JsonResult GetAllUsers([DataSourceRequest]DataSourceRequest request)
        {
            var context = new ApplicationDbContext();

            var users = context.Users.Where(usr => usr.UserName != null).ToList().Select(u => new UserModelAdmin()
            {
                Id = u.Id,
                Username = u.UserName,
                Food = u.Food,
                Wood = u.Wood,
                Iron = u.Iron,
                Population = u.Population
            });

            return Json(users.ToDataSourceResult(request, ModelState));
        }
        public ActionResult EditUser([DataSourceRequest] DataSourceRequest request, UserModelAdmin usr)
        {
            var context = new ApplicationDbContext();

            if (usr != null && ModelState.IsValid)
	        {
		        var user = context.Users.FirstOrDefault(u => u.Id == usr.Id);
                if (user != null)
                {
                    user.UserName = usr.Username;
                    user.Wood = usr.Wood;
                    user.Food = usr.Food;
                    user.Iron = usr.Iron;
                    user.Population = usr.Population;

                    context.SaveChanges();
                }
	        }
            return Json(new[] { usr }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Private Methods
        private string SecondsToTime(int seconds)
        {
            string time = string.Format("{0:00}:{1:00}:{2:00}",
                seconds / 3600, (seconds / 60) % 60, seconds % 60);

            return time;
        }

        private int TimeToSeconds(string time)
        {
            string[] separatedTime = time.Split(':');
            return Convert.ToInt32(separatedTime[2]) + Convert.ToInt32(separatedTime[1]) * 60 +
                Convert.ToInt32(separatedTime[0]) * 3600;
        }

        #endregion
    }
}