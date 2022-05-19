using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Zalozba
    {
        public int ZalozbaID { get; set; }
        public string Naziv { get; set; }   // Mladinska knjiga...
        public string TelefonskaStevilka { get; set; }
        public string Naslov { get; set; }

        public ICollection<Gradivo>? Gradiva { get; set; }
    }
}