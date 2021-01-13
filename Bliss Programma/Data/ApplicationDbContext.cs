using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bliss_Programma.Models;

namespace Bliss_Programma.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bliss_Programma.Models.Locatie> Locatie { get; set; }
        public DbSet<Bliss_Programma.Models.Ruimte> Ruimte { get; set; }
        public DbSet<Bliss_Programma.Models.Reservering> Reservering { get; set; }
    }
}
