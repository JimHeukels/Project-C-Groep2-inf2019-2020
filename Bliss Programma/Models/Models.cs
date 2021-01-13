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
        public string WerknemerEmail { get; set; }

        [ForeignKey("Ruimte")]
        public int RuimteId { get; set; }
        public Ruimte Ruimte { get; set; }

        public Reservering()
        {

        }
    }
}