using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Entities.IdentityModels
{
    public class UserRoles : IdentityUserRole<string>
    {
        public bool IsActive { get; set; }

    }
}
