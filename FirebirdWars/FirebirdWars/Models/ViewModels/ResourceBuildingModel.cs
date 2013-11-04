using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class ResourceBuildingModel
    {
        public string Type { get; set; }

        public int CurrentProduction { get; set; }

        public int NextLevelProduction { get; set; }

        public string ResourceType { get; set; }
    }
}