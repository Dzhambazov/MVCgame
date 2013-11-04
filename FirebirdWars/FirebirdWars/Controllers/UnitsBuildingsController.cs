using FirebirdWars.Models;
using FirebirdWars.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FirebirdWars.Controllers
{
    public class UnitsBuildingsController : BuildingsController
    {
        public ActionResult GetAvailableUnits(int id)
        {
            var buildingId = id;

            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            var units = (from ur in context.UnitRequirements.ToList()
                         join u in context.Units.ToList() on ur.UnitId equals u.Id into urGroup
                         from u in urGroup.DefaultIfEmpty()
                         where ur.RequiredBuildingId == buildingId
                         select new AvailableUnitsModel()
                         {
                             Id = u.Id,
                             Type = u.Type,
                             WoodCost = u.WoodCost,
                             FoodCost = u.FoodCost,
                             IronCost = u.IronCost,
                             PopulationCost = u.PopulationCost,
                             Speed = u.Speed,
                             CarryingCapacity = u.CarryingCapacity,
                             BuildTime = SecondsToTime(u.BuildTime),
                             InfantryPower = u.InfantryPower,
                             CavalryPower = u.CavalryPower,
                             SiegeArtilleryPower = u.SiegeArtilleryPower,
                             HealthPoints = u.HealthPoints,
                             MaxUnits = GetMaxUnitsToBuy(u, user)
                         });

            return PartialView("BuildingsPartials/_AvailableUnitsPartial", units);
        }

        public ActionResult GetUnitsInProgress(int id)
        {
            var buildingId = id;

            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);
            
            var units = from uip in context.UnitsInProgress.ToList()
                        join u in context.Units.ToList() on uip.UnitId equals u.Id into uGroup
                        from u in uGroup.DefaultIfEmpty()
                        join ur in context.UnitRequirements.ToList() on uip.UnitId equals ur.UnitId into urGroup
                        from ur in urGroup.DefaultIfEmpty()
                        where ur.RequiredBuildingId == buildingId && uip.UserId == user.Id
                        select new UserUnitsInProgressModel()
                        {
                            Id = uip.Id,
                            Type = uip.Unit.Type,
                            CompletionDate = uip.DateEnded,
                            Quantity = uip.Quantity
                        }; 

            //var units = from ur in context.UnitRequirements.ToList()
            //            join u in context.Units.ToList() on ur.UnitId equals u.Id into urGroup
            //            from u in urGroup.DefaultIfEmpty()
            //            join uip in context.UnitsInProgress on u.Id equals uip.UnitId into uipGroup
            //            from uip in uipGroup.DefaultIfEmpty()
            //            where ur.RequiredBuildingId == buildingId && uip.UserId == user.Id
            //            select new UserUnitsInProgressModel()
            //            {
            //                Id = uip.Id,
            //                Type = uip.Unit.Type,
            //                CompletionDate = uip.DateEnded,
            //                Quantity = uip.Quantity
            //            };

            return PartialView("BuildingsPartials/_UserUnitsInProgress", units);
        }

        public ActionResult RecruitUnit(int unitsCount, int unitId)
        {
            var context = new ApplicationDbContext();
            string userGuidId = this.User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            if (unitsCount <= 0)
            {
                return HttpNotFound();
            }

            var unit = context.Units.Find(unitId);

            if (unit == null)
            {
                return HttpNotFound();
            }

            var totalBuildTime = unit.BuildTime * unitsCount;

            var dateEnded = DateTime.Now.AddSeconds(totalBuildTime);

            if (GetMaxUnitsToBuy(unit, user) >= unitsCount)
            {
                // tax that bitch
                user.Wood -= unit.WoodCost * unitsCount;
                user.Food -= unit.FoodCost * unitsCount;
                user.Iron -= unit.IronCost * unitsCount;
                user.Population += unit.PopulationCost * unitsCount;

                user.UnitsInProgress.Add(new UnitsInProgress()
                {
                    Unit = unit,
                    User = user,
                    DateEnded = dateEnded,
                    Quantity = unitsCount
                });

                context.SaveChanges();
            }

            return RedirectToAction("Building/2", "Buildings");
        }

        public ActionResult CancelCreateUnit(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var unitInProgress = context.UnitsInProgress.FirstOrDefault(uip => uip.Id == id);

            if (unitInProgress == null)
            {
                return HttpNotFound();
            }

            string userGuidId = User.Identity.GetUserId();
            var user = context.Users.FirstOrDefault(u => u.Id == userGuidId);

            user.UnitsInProgress.Remove(unitInProgress);
            context.UnitsInProgress.Remove(unitInProgress);

            context.SaveChanges();

            return RedirectToAction("Building/2", "Buildings");
        }
    }
}