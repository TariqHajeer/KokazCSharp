using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class UserPhone
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }

        public virtual User User { get; set; }
    }
}
