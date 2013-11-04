using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirebirdWars.Areas.Administration.Models.ViewModels
{
    public class UserModelAdmin
    {
        [Editable(false)]
        public string Id { get; set; }
        [Editable(false)]
        public string Username { get; set; }
        public int Wood { get; set; }

        public int Iron { get; set; }

        public int Food { get; set; }
        
        public int Population { get; set; }
    }
}