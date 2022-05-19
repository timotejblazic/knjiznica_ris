using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace web.Models
{
    public class Uporabnik : IdentityUser
    {
        // public int UporabnikID { get; set; }
        public string Ime { get; set; }
        public string Priimek { get; set; }
        // public string UporabniskoIme { get; set; }
        // public string TelefonskaStevilka { get; set; }
        public DateTime CasVpisa { get; set; }
        // iz tistga extensiona se bo dalo se geslo pa to da bo secure

        //many
        public ICollection<Nakup>? Nakupi { get; set; }
        public ICollection<Izposoja>? Izposoje { get; set; }
        public ICollection<Ocena>? Ocene { get; set; }
    }
}