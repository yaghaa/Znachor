using System.Linq;

namespace Znachor.Models
{
  using System.Data.Entity;

  public class Znachor : DbContext
  {
    public Znachor()
        : base("name=Znachor")
    {
    }

    public virtual DbSet<Adresprzesylki> Adresprzesylkis { get; set; }
    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
    public virtual DbSet<dbo_RodzajPrzesylki> dbo_RodzajPrzesylki { get; set; }
    public virtual DbSet<Kategoria> Kategorias { get; set; }
    public virtual DbSet<Kurier> Kuriers { get; set; }
    public virtual DbSet<PosiadaKategorie> PosiadaKategories { get; set; }
    public virtual DbSet<Przesylka> Przesylkas { get; set; }
    public virtual DbSet<Rachunek> Rachuneks { get; set; }
    public virtual DbSet<RodzajPlatnosci> RodzajPlatnoscis { get; set; }
    public virtual DbSet<Towar> Towars { get; set; }
    public virtual DbSet<Zamowienie> Zamowienies { get; set; }
    public virtual DbSet<Koszyk> Koszyks { get; set; }
    public virtual DbSet<ZamowionyTowar> ZamowionyTowars { get; set; }
    public virtual DbSet<ZamowionyTowar2> ZamowionyTowar2 { get; set; }

    public virtual Koszyk GetKoszyk(int id)
    {
      return this.Koszyks.FirstOrDefault(x => x.Towarid_towaru == id);
    }

    public virtual Koszyk GetKoszyk(int id, string userId)
    {
      return this.Koszyks.FirstOrDefault(x => x.Towarid_towaru == id && x.AspNetUsersid == userId);
    }

    public virtual IQueryable<Koszyk> GetKoszykWhere(string id)
    {
      return this.Koszyks.Where(x => x.AspNetUsersid == id);
    }

    public virtual Towar GetFirstTowar(int id)
    {
      return this.Towars.First(x => x.id_towaru == id); ;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<dbo_RodzajPrzesylki>()
          .Property(e => e.koszt_przesylki)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Kategoria>()
          .HasMany(e => e.Kategoria1)
          .WithOptional(e => e.Kategoria2)
          .HasForeignKey(e => e.Kategoriaid_kategorii);

      modelBuilder.Entity<Kurier>()
          .Property(e => e.koszt)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Przesylka>()
          .Property(e => e.czy_dostarczona)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.towary_netto)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.przesylka_netto)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.suma_vat)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.rabat)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.suma_brutto)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Rachunek>()
          .Property(e => e.czy_zaplacony)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Towar>()
          .Property(e => e.cena_netto)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Zamowienie>()
          .Property(e => e.nr_rachunku)
          .IsUnicode(false);
    }
  }
}