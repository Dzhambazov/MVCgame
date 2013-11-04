using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class UserBuildingsInProgressModel
    {
        public static Expression<Func<BuildingsInProgress, UserBuildingsInProgressModel>> FromBuilding
        {
            get
            {
                return building => new UserBuildingsInProgressModel()
                {
                    Id = building.Id,
                    Type = building.Building.Type,
                    CompletionDate = building.DateEnded
                };
            }
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime CompletionDate { get; set; }
    }
}