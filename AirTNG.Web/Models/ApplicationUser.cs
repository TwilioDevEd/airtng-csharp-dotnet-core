using System;
using Microsoft.AspNetCore.Identity;

namespace AirTNG.Web.Models
{
    public class ApplicationUser:IdentityUser
    {

        public string Name { get; set; }

    }
}