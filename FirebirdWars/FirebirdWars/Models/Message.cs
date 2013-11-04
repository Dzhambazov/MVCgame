using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebirdWars.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public int FromUserId { get; set; }
        public virtual ApplicationUser FromUser { get; set; }

        public int ToUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }

    }
}