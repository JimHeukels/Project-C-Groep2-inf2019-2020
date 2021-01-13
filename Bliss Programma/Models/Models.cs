using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bliss_Programma.Models
{
    public class Locatie
    {
        [Key]
        public int Id { get; set; }

        public string Straatnaam { get; set; }
        public int Nummer { get; set; }
        public string Toevoeging { get; set; }
        public string Postcode { get; set; }
        public string Plaatsnaam { get; set; }
        public List<Ruimte> Werkplekken { get; set; }

        public Locatie()
        {

        }
    }

    public class Ruimte
    {
        [Key]
        public int Id { get; set; }

        public int Lengte { get; set; }
        public int Breedte { get; set; }
        public int Oppervlakte { get; set; }
        public int MaxWerkplekken { get; set; }
        public string Naam { get; set; }
        public List<Reservering> Reserveringen { get; set; }

        [ForeignKey("Locatie")]
        public int LocatieId { get; set; }
        public Locatie Locatie { get; set; }

        public Ruimte()
        {

        }
    }

    public class Reservering
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datum { get; set; }

        public string WerknemerId { get; set; }

        [ForeignKey("Ruimte")]
        public int RuimteId { get; set; }
        public Ruimte Ruimte { get; set; }

        public Reservering()
        {

        }
    }
    public class reservemodel
    {
        public DateTime Date { get; set; }
    }
    public class AllUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Prioriteit { get; set; }
    }
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Prioriteit{ get; set; }
    }
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        public string Prioriteit { get; set; }

        public bool IsAdmin { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}