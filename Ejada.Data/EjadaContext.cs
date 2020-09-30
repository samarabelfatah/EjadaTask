using Ejada.Entities.IdentityModels;
using Ejada.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Data
{
        public class EjadaContext : IdentityDbContext<User, Role, string,
           IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>,
           IdentityRoleClaim<string>, IdentityUserToken<string>>
        {
            public EjadaContext(DbContextOptions<EjadaContext> options) : base(options)
            {

            }
            public DbSet<Employee> Employee { get; set; }
            public DbSet<Department> Department { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }


    }
}
