using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class BuildingRequirementsModel
    {
        public int Id { get; set; }

        public string BuildingType { get; set; }

        public int BuildingLevel { get; set; }
    }
}