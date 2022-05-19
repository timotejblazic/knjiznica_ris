using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Gradivo
    {
        public int GradivoID { get; set; }
        public string Naslov { get; set; }
        public int LetoIzdaje { get; set; }
        public int SteviloStrani { get; set; }
        public string Opis { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaGradivo { get; set; }

        //one
        [ForeignKey("Kategorija")]
        public int? KategorijaID { get; set; }
        public Kategorija? Kategorija { get; set; }
        [ForeignKey("Zanr")]
        public int? ZanrID { get; set; }
        public Zanr? Zanr { get; set; }
        [ForeignKey("Zalozba")]
        public int? ZalozbaID { get; set; }
        public Zalozba? Zalozba { get; set; }
        [ForeignKey("Avtor")]
        public int? AvtorID { get; set; }
        public Avtor? Avtor { get; set; }

        //many
        public ICollection<GradivoIzvod>? GradivoIzvodi { get; set; }
        public ICollection<Ocena>? Ocene { get; set; }
    }
}