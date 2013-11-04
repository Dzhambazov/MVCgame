using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class UserBuildingModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public int RequiredWood { get; set; }

        public int RequiredFood { get; set; }

        public int RequiredIron { get; set; }

        public int RequiredPopulation { get; set; }

        public string BuildTime { get; set; }
    }
}