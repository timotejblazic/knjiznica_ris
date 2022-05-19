using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class GradivoIzvod
    {
        public int GradivoIzvodID { get; set; }

        //one
        [ForeignKey("Gradivo")]
        public int? GradivoID { get; set; }
        public Gradivo? Gradivo { get; set; }
        [ForeignKey("Izposoja")]
        public int? IzposojaID { get; set; }
        public Izposoja? Izposoja { get; set; }
        [ForeignKey("Nakup")]
        public int? NakupID { get; set; }
        public Nakup? Nakup { get; set; }
    
    }
}