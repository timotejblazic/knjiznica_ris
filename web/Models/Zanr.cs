using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Zanr
    {
        public int ZanrID { get; set; }
        public string Naziv { get; set; }   // komedija, drama,...

        public ICollection<Gradivo>? Gradiva { get; set; }
    }
}