using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Ejada.Entities.IdentityModels
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public byte[] GWTPasswordHash { get; set; }
        public byte[] GWTPasswordSalt { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}

