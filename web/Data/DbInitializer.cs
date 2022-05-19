using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(KnjiznicaContext context)
        {
            context.Database.EnsureCreated();

            if(context.Zalozbe.Any()){
                return;
            }
            var zalozbe= new Zalozba[]{
                new Zalozba{Naziv="Mladinska knjiga",TelefonskaStevilka="031000001",Naslov="Šmartinska cesta 6, 1000 Ljubljana"},
                new Zalozba{Naziv="Sanje",TelefonskaStevilka="031000002",Naslov="Dolenjska cesta 76, 2000 Maribor"},
                new Zalozba{Naziv="Miš",TelefonskaStevilka="031000003",Naslov="Titova ulica 82, 8341 Adlešiči"},
                new Zalozba{Naziv="Primus",TelefonskaStevilka="031000004",Naslov="Cesta Lejzov 35, 1353 Borovnica"},
                new Zalozba{Naziv="Zala",TelefonskaStevilka="031000005",Naslov="Blejska ulic 9, 4260 Bled"}
            };
            foreach (Zalozba z in zalozbe)
            {
                context.Zalozbe.Add(z);
            }

            context.SaveChanges();

            var zanri= new Zanr[]{
                new Zanr{Naziv="komedija"},
                new Zanr{Naziv="drama"},
                new Zanr{Naziv="grozljivka"},
                new Zanr{Naziv="tragedija"},
                new Zanr{Naziv="potopis"},
            };
            foreach (Zanr za in zanri)
            {
                context.Zanri.Add(za);
            }

            context.SaveChanges();

            var kategorije=new Kategorija[]{
                new Kategorija{Naziv="knjiga"},
                new Kategorija{Naziv="strip"},
                new Kategorija{Naziv="roman"},
                new Kategorija{Naziv="slovar"},
                new Kategorija{Naziv="učbenik"}
            };
            foreach (Kategorija k in kategorije)
            {
                context.Kategorije.Add(k);
            }

            context.SaveChanges();

            var avtorji=new Avtor[]{
                new Avtor{Ime="Janez",Priimek="Svetokriški",Opis="Slovenski avtor rojen 17.2.1955 v Brdah na Koroškem."},
                new Avtor{Ime="John",Priimek="Green",Opis="Ameriški avtor rojen 7.8.1833."},
                new Avtor{Ime="Johaness",Priimek="Krakow",Opis="Poljski romanopisec rojen 8.12.1977."},
                new Avtor{Ime="Silvo",Priimek="Milkić",Opis="Srbski poznavalec jezikov."},
                new Avtor{Ime="Karen",Priimek="Hamsburg",Opis="Nemška učiteljica kemije."}
            };
            foreach (Avtor a in avtorji)
            {
                context.Avtorji.Add(a);
            }

            context.SaveChanges();

            var gradiva=new Gradivo[]{
                new Gradivo{Naslov="Alica v čudežni deželi",LetoIzdaje=2005,SteviloStrani=168,Opis="Alica se odpravi v čudežno deželo, kjer...",KategorijaID=1, ZanrID=2,ZalozbaID=3,AvtorID=1,CenaGradivo=22.12M},
                new Gradivo{Naslov="Asterix in Obelix",LetoIzdaje=1999,SteviloStrani=53,Opis="Sledi Asterixu in Obelixu na njunih dogodivščinah",KategorijaID=2, ZanrID=1,ZalozbaID=2,AvtorID=2,CenaGradivo=42.53M},
                new Gradivo{Naslov="Ta veseli dan",LetoIzdaje=1966,SteviloStrani=300,Opis="Ta veseli dan ali Matiček se ženi govori o...",KategorijaID=3, ZanrID=2,ZalozbaID=5,AvtorID=3,CenaGradivo=12.99M}
            };
            foreach (Gradivo g in gradiva)
            {
                context.Gradiva.Add(g);
            }

            context.SaveChanges();







            var roles = new IdentityRole[] {
                new IdentityRole{Id="1",Name="Administrator"},
                new IdentityRole{Id="2",Name="Moderator"}
            };
            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();

            var user = new Uporabnik
            {
                Id = "1",
                Ime = "Janez",
                Priimek = "Novak",
                CasVpisa = DateTime.Now,
                Email = "janez.novak@gmail.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "janez.novak@gmail.com",
                NormalizedUserName = "janez.novak@gmail.com",
                PhoneNumber = "041817193",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<Uporabnik>();
                var hashed = password.HashPassword(user, "Novak12_");
                user.PasswordHash = hashed;
                context.Users.Add(user);
            }

            context.SaveChanges();

            var UserRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=user.Id},
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }

            context.SaveChanges();








            var ocene=new Ocena[]{
                new Ocena{Vrednost=3,Mnenje="Super knjiga!",UporabnikID="1",GradivoID=1},
                new Ocena{Vrednost=5,Mnenje="Moje mnenje",UporabnikID="1",GradivoID=2},
                new Ocena{Vrednost=1,Mnenje="Priporočam za začetne bralce!",UporabnikID="1",GradivoID=3},
            };
            foreach (Ocena o in ocene)
            {
                context.Ocene.Add(o);
            }

            context.SaveChanges();


            var izposoje=new Izposoja[]{
                new Izposoja{DatumIzposoje=DateTime.Now, DatumVrnitve=DateTime.Now.AddDays(14), IdIzposojenegaGradiva=1,UporabnikID="1"},
            };
            foreach (Izposoja i in izposoje){
                context.Izposoje.Add(i);
            }

            context.SaveChanges();

            var nakupi=new Nakup[]{
                new Nakup{DatumNakupa=DateTime.Now,IdKupljenegaGradiva=1,UporabnikID="1"}
            };
            foreach (Nakup n in nakupi){
                context.Nakupi.Add(n);
            }


            context.SaveChanges();



            var gradivaIzvodi=new GradivoIzvod[]{
                new GradivoIzvod{GradivoID=1},
                new GradivoIzvod{GradivoID=2},
                new GradivoIzvod{GradivoID=3},
                new GradivoIzvod{GradivoID=3,NakupID=1},
                new GradivoIzvod{GradivoID=2,IzposojaID=1}
            };
            foreach (GradivoIzvod gi in gradivaIzvodi)
            {
                context.GradivoIzvodi.Add(gi);
            }

            context.SaveChanges();

            



            
        }
    }
}