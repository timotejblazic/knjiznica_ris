using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Avtor
    {
        public int AvtorID { get; set; }
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string PolnoIme { get{ return Ime + " " + Priimek;} }
        public string Opis { get; set; }

        public ICollection<Gradivo>? Gradiva { get; set; }
    }
}