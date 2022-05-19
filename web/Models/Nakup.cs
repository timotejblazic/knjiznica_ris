using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Nakup
    {
        public int NakupID { get; set; }
        public DateTime DatumNakupa { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public int IdKupljenegaGradiva { get; set; }

        //one
        [ForeignKey("Uporabnik")]
        public string? UporabnikID { get; set; }
        public Uporabnik? Uporabnik { get; set; }

        // one to one
        public GradivoIzvod? GradivoIzvod { get; set; }
    }
}