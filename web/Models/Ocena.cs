using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Ocena
    {
        public int OcenaID { get; set; }
        [Range(1,5)]
        public int Vrednost { get; set; }    // 1,2,3,4,5
        public string? Mnenje { get; set; }

        //one
        [ForeignKey("Uporabnik")]
        public string? UporabnikID { get; set; }
        public Uporabnik? Uporabnik { get; set; }
        [ForeignKey("Gradivo")]
        public int? GradivoID { get; set; }
        public Gradivo? Gradivo { get; set; }
    }
}