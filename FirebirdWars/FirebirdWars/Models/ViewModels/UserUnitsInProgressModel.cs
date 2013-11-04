using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class UserUnitsInProgressModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime CompletionDate { get; set; }

        public int Quantity { get; set; }
    }
}