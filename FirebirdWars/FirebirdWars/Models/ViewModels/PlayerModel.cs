using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models.ViewModels
{
    public class PlayerModel
    {
        public string Id { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }
        public int Distance { get; set; }
    }
}