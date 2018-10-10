using System;
using System.Collections.Generic;
using System.Text;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirTNG.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<VacationProperty> VacationProperties { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}