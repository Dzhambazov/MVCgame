using FirebirdWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using FirebirdWars.Models.ViewModels;
using FirebirdWars.Models.Enums;

namespace FirebirdWars.Controllers
{
    public class BuildingsController : Controller
    {
        protected const double ResourceCoeficient = 1.267;

        protected const double BuildTimeCoeficient = 1.667;

        protected const int MaxBuildingsInProgress = 2;

        protected const double ResourceProductionCoeficient = 1.15;

        protected const int InitialPopulationIncome = 50;
       
        //
        // GET: /Buildings/id/
        public ActionResult Building(int id, string buildingType)
        {
            var context = new ApplicationDbContext();

            var building = context.Buildings.Find(id);

            if (building != null)
            {
                return View(building.Type, building);
            }

            return RedirectToAction("Index", "Home");
        }

        public void CheckForFinishedUnits(string userId)
        {
            var context = new ApplicationDbContext();

            string userGuidId = userId;
            var currentUser = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var unfinishedUnits = currentUser
                        .UnitsInProgress.Where(uip => uip.DateEnded <= DateTime.Now);

            if (unfinishedUnits.Count() > 0)
            {
                foreach (var unit in unfinishedUnits)
                {
                    var unitExists = currentUser.Units.FirstOrDefault(u => u.UnitId == unit.UnitId);

                    if (unitExists != null)
                    {
                        unitExists.UnitCount += unit.Quantity;
                    }
                    else
                    {
                        currentUser.Units.Add(new UserUnit()
                        {
                            Unit = unit.Unit,
                            User = currentUser,
                            UnitCount = unit.Quantity
                        });
                    }
                }

                context.UnitsInProgress.RemoveRange(unfinishedUnits);

                context.SaveChanges();
            }
        }

        public void CheckForFinishedBuildings(string userId)
        {
            var context = new ApplicationDbContext();

            string userGuidId = userId;
            var currentUser = context.Users.FirstOrDefault(u => u.Id == userGuidId);


            var unfinishedBuildings = currentUser.BuildingsInProgress.Where(bip => bip.DateEnded <= DateTime.Now);

            if (unfinishedBuildings.Count() > 0)
            {
                foreach (var building in unfinishedBuildings)
                {
                    var buildingExist = currentUser.Buildings.FirstOrDefault(b => b.BuildingId == building.BuildingId);

                    if (buildingExist != null)
                    {
                        //Upgrade it
                        buildingExist.BuildingLevel++;

                        if (buildingExist.Building.Type == "House")
                        {
                            currentUser.MaxPopulation += InitialPopulationIncome * buildingExist.BuildingLevel;
                        }
                    }
                    else
                    {
                        //Add it
                        currentUser.Buildings.Add(new UserBuilding()
                        {
                            Building = building.Building,
                            BuildingLevel = 1,
                            User = currentUser
                        });

                        if (building.Building.Type == "House")
                        {
                            currentUser.MaxPopulation += InitialPopulationIncome;
                        }
                    }

                    //currentUser.BuildingsInProgress.Remove(building);
                    //context.BuildingsInProgress.Remove(building);
                }

                context.BuildingsInProgress.RemoveRange(unfinishedBuildings);

                context.SaveChanges();
            }
        }

        public void UpdateResources(string userId)
        {
            var context = new ApplicationDbContext();

            string userGuidId = userId;
            var currentUser = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var difference = (DateTime.Now - currentUser.LastOnline).Value.TotalSeconds;

            if (difference < 60)
            {
                return;
            }

            var lumberjacks = currentUser.Buildings.FirstOrDefault(b => b.Building.Type == "Lumberjacks");
            var farm = currentUser.Buildings.FirstOrDefault(b => b.Building.Type == "Farm");
            var ironMine = currentUser.Buildings.FirstOrDefault(b => b.Building.Type == "Iron Mine");

            var currentWoodProduction = Calculate((int)BuildingsIncome.Wood, GetCoeficient(ResourceProductionCoeficient, lumberjacks.BuildingLevel - 1));
            var currentFoodProduction = Calculate((int)BuildingsIncome.Wood, GetCoeficient(ResourceProductionCoeficient, farm.BuildingLevel - 1));
            var currentIronProduction = Calculate((int)BuildingsIncome.Wood, GetCoeficient(ResourceProductionCoeficient, ironMine.BuildingLevel - 1));

            var woodIncome = Convert.ToInt32(Math.Round(currentWoodProduction * (difference / 3600), MidpointRounding.AwayFromZero));
            var foodIncome = Convert.ToInt32(Math.Round(currentFoodProduction * (difference / 3600), MidpointRounding.AwayFromZero));
            var ironIncome = Convert.ToInt32(Math.Round(currentIronProduction * (difference / 3600), MidpointRounding.AwayFromZero));

            currentUser.Wood += woodIncome;
            currentUser.Food += foodIncome;
            currentUser.Iron += ironIncome;

            // update current user's last time he was given resources
            currentUser.LastOnline = DateTime.Now;

            context.SaveChanges();
        }

        protected ApplicationUser GetUser()
        {
            
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            return user;
        }

        protected int GetMaxUnitsToBuy(Unit unit, ApplicationUser user) 
        {
            int availableUnits = 0;

            var userWood = user.Wood;
            var userFood = user.Food;
            var userIron = user.Iron;
            var userCurrentPopulation = user.Population;

            while (userWood >= unit.WoodCost &&
                userFood >= unit.FoodCost &&
                userIron >= unit.IronCost &&
                user.MaxPopulation > unit.PopulationCost + userCurrentPopulation)
            {
                userWood -= unit.WoodCost;
                userFood -= unit.FoodCost;
                userIron -= unit.IronCost;
                userCurrentPopulation += unit.PopulationCost;
                availableUnits++;
            }

            return availableUnits;
        }

        protected string SecondsToTime(int seconds)
        {
            string time = string.Format("{0:00}:{1:00}:{2:00}",
                seconds / 3600, (seconds / 60) % 60, seconds % 60);

            return time;
        }

        protected double GetCoeficient(double coeficient, int level)
        {
            double coef = Math.Pow(coeficient, level);

            return coef;
        }

        protected int Calculate(int value, double coeficient)
        {
            double result = value * coeficient;

            return (int)result;
        }
	}
}