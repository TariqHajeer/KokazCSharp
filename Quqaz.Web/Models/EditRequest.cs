﻿using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class EditRequest: IHaveBranch,IIdEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
        public int? UserId { get; set; }
        public bool? Accept { get; set; }

        public virtual Client Client { get; set; }
        public virtual User User { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
