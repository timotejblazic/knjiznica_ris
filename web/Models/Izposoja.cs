using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Izposoja
    {
        public int IzposojaID { get; set; }
        public DateTime DatumIzposoje { get; set; }
        public DateTime DatumVrnitve { get; set; }
        public int IdIzposojenegaGradiva { get; set; }
        
        //one
        [ForeignKey("Uporabnik")]
        public string? UporabnikID { get; set; }
        public Uporabnik? Uporabnik { get; set; }

        // one to one
        public GradivoIzvod? GradivoIzvod { get; set; }
    }
}