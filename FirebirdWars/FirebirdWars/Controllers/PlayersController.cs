using FirebirdWars.Models;
using FirebirdWars.Models.ViewModels;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;

namespace FirebirdWars.Controllers
{
    public class PlayersController : Controller
    {
        protected const double ResourceCoeficient = 1.267;

        protected const int PointsDevider = 100;

        public ActionResult PlayersTop()
        {
            return View("PlayersTop");
        }

        public JsonResult GetTopPlayers([DataSourceRequest]DataSourceRequest request)
        {
            var context = new ApplicationDbContext();

            string currentPlayerId = User.Identity.GetUserId();

            int currentPlayerLocationX = context.Users.FirstOrDefault(x => x.Id == currentPlayerId).LocationX;
            int currentPlayerLocationY = context.Users.FirstOrDefault(x => x.Id == currentPlayerId).LocationY;

            var players = context.Users.ToList().Select(player => new PlayerModel()
            {
                Id = player.Id,
                PlayerName = player.UserName,
                Points = (CalcBuildingPoints(player.Buildings.ToList()) + CalcUnitsPoints(player.Units.ToList())),
                Distance = CalcDistanceBetweenPlayers(currentPlayerLocationX, currentPlayerLocationY, player.LocationX, player.LocationY)
            }).ToList().OrderByDescending(x => x.Points);

            int rank = 0;
            foreach (var player in players)
            {
                player.Rank = ++rank;
            }
            
            return Json(players.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private int CalcUnitsPoints(List<UserUnit> units)
        {
            int points = 0;
            foreach (var unit in units)
            {
                points += unit.UnitCount * (unit.Unit.WoodCost + unit.Unit.FoodCost + unit.Unit.IronCost);
            }
            return points / PointsDevider;
        }
        private int CalcBuildingPoints(IEnumerable<UserBuilding> buildings)
        {
            int points = 0;
            foreach (var building in buildings)
            {
                points += GetBuildingPoints(building);
            }
            return points;
        }

        private int GetBuildingPoints(UserBuilding building)
        {
            double points = 0;
            for (int i = 1; i <= building.BuildingLevel; i++)
            {
                points += ((Math.Pow(ResourceCoeficient, i) * building.Building.WoodCost) + 
                    (Math.Pow(ResourceCoeficient,i) * building.Building.IronCost) + 
                    (Math.Pow(ResourceCoeficient,i) * building.Building.FoodCost));
            }

            return (int)Math.Round(points / PointsDevider);
        }

        private int CalcDistanceBetweenPlayers(int x1, int y1, int x2, int y2)
        {
            int result = (int)(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            return (int)Math.Sqrt(result);
        }
	}
}