using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class KnjiznicaContext : IdentityDbContext<Uporabnik>
    {
        public KnjiznicaContext(DbContextOptions<KnjiznicaContext> options) : base(options)
        {
        }

        public DbSet<Avtor> Avtorji { get; set; }
        public DbSet<Gradivo> Gradiva { get; set; }
        public DbSet<GradivoIzvod> GradivoIzvodi { get; set; }
        public DbSet<Izposoja> Izposoje { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Nakup> Nakupi { get; set; }
        public DbSet<Ocena> Ocene { get; set; }
        public DbSet<Uporabnik> Uporabniki { get; set; }
        public DbSet<Zalozba> Zalozbe { get; set; }
        public DbSet<Zanr> Zanri { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Avtor>().ToTable("Avtor");
            modelBuilder.Entity<Gradivo>().ToTable("Gradivo");
            modelBuilder.Entity<GradivoIzvod>().ToTable("GradivoIzvod");
            modelBuilder.Entity<Izposoja>().ToTable("Izposoja");
            modelBuilder.Entity<Kategorija>().ToTable("Kategorija");
            modelBuilder.Entity<Nakup>().ToTable("Nakup");
            modelBuilder.Entity<Ocena>().ToTable("Ocena");
            modelBuilder.Entity<Uporabnik>().ToTable("Uporabnik");
            modelBuilder.Entity<Zalozba>().ToTable("Zalozba");
            modelBuilder.Entity<Zanr>().ToTable("Zanr");


            // Spremeni ON DELETE na RESTRICT, da ni na CASCADE... moras tudi dodati nov Migration
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder); // to je tudi pri VAJA4
        }
    }
}