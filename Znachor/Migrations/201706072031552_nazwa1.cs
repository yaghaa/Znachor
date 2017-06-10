namespace Znachor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nazwa1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresprzesylki",
                c => new
                    {
                        id_adresu = c.Int(nullable: false, identity: true),
                        miejscowosc = c.String(nullable: false, maxLength: 40),
                        ulica = c.String(nullable: false, maxLength: 50),
                        nrdomu = c.Int(nullable: false),
                        nrlokalu = c.Int(),
                        kodpocztowy = c.String(nullable: false, maxLength: 6),
                    })
                .PrimaryKey(t => t.id_adresu);
            
            CreateTable(
                "dbo.Przesylka",
                c => new
                    {
                        id_przesylki = c.Int(nullable: false, identity: true),
                        id_adresu = c.Int(nullable: false),
                        id_kuriera = c.Int(nullable: false),
                        czy_dostarczona = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        data_wyslania = c.DateTime(),
                        data_dostarczenia = c.DateTime(),
                        rodzaj = c.String(nullable: false, maxLength: 30),
                        Adresprzesylkiid_adresu = c.Int(nullable: false),
                        Kurierid_kuriera = c.Int(nullable: false),
                        dboRodzajPrzesylkirodzaj = c.String(name: "dbo.RodzajPrzesylkirodzaj", nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.id_przesylki)
                .ForeignKey("dbo.Adresprzesylki", t => t.Adresprzesylkiid_adresu, cascadeDelete: true)
                .Index(t => t.Adresprzesylkiid_adresu);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        AspNetRolesid = c.String(nullable: false, maxLength: 128),
                        AspNetUsersid = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetRolesid, t.AspNetUsersid });
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.[dbo.RodzajPrzesylki]",
                c => new
                    {
                        rodzaj = c.String(nullable: false, maxLength: 30),
                        koszt_przesylki = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.rodzaj);
            
            CreateTable(
                "dbo.Kategoria",
                c => new
                    {
                        id_kategorii = c.Int(nullable: false, identity: true),
                        Kat_id_kategorii = c.Int(),
                        Kategoriaid_kategorii = c.Int(),
                    })
                .PrimaryKey(t => t.id_kategorii)
                .ForeignKey("dbo.Kategoria", t => t.Kategoriaid_kategorii)
                .Index(t => t.Kategoriaid_kategorii);
            
            CreateTable(
                "dbo.PosiadaKategorie",
                c => new
                    {
                        Kategoriaid_kategorii = c.Int(nullable: false),
                        Towarid_towaru = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Kategoriaid_kategorii, t.Towarid_towaru })
                .ForeignKey("dbo.Kategoria", t => t.Kategoriaid_kategorii, cascadeDelete: true)
                .Index(t => t.Kategoriaid_kategorii);
            
            CreateTable(
                "dbo.Koszyk",
                c => new
                    {
                        AspNetUsersid = c.String(nullable: false, maxLength: 128),
                        Towarid_towaru = c.Int(nullable: false),
                        ilosc_sztuk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AspNetUsersid, t.Towarid_towaru, t.ilosc_sztuk });
            
            CreateTable(
                "dbo.Kurier",
                c => new
                    {
                        id_kuriera = c.Int(nullable: false, identity: true),
                        nazwa = c.String(nullable: false, maxLength: 50),
                        koszt = c.Decimal(nullable: false, storeType: "money"),
                        max_czas_dostawy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_kuriera);
            
            CreateTable(
                "dbo.Rachunek",
                c => new
                    {
                        id_rachunku = c.Int(nullable: false, identity: true),
                        towary_netto = c.Decimal(nullable: false, storeType: "money"),
                        przesylka_netto = c.Decimal(nullable: false, storeType: "money"),
                        suma_vat = c.Decimal(nullable: false, storeType: "money"),
                        rabat = c.Decimal(nullable: false, storeType: "money"),
                        suma_brutto = c.Decimal(nullable: false, storeType: "money"),
                        czy_zaplacony = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.id_rachunku);
            
            CreateTable(
                "dbo.RodzajPlatnosci",
                c => new
                    {
                        rodzaj = c.String(nullable: false, maxLength: 30),
                        szczegoly = c.String(nullable: false, maxLength: 255),
                        nr_konta = c.String(nullable: false, maxLength: 26),
                    })
                .PrimaryKey(t => t.rodzaj);
            
            CreateTable(
                "dbo.Towar",
                c => new
                    {
                        id_towaru = c.Int(nullable: false, identity: true),
                        nazwa = c.String(nullable: false, maxLength: 50),
                        cena_netto = c.Decimal(nullable: false, storeType: "money"),
                        ilosc_w_magazynie = c.Int(nullable: false),
                        producent = c.String(nullable: false, maxLength: 40),
                        forma = c.String(nullable: false, maxLength: 25),
                        sklad = c.String(nullable: false, maxLength: 255),
                        szczegoly = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.id_towaru);
            
            CreateTable(
                "dbo.Zamowienie",
                c => new
                    {
                        id_zamowienia = c.Int(nullable: false, identity: true),
                        nr_rachunku = c.String(nullable: false, maxLength: 128, unicode: false),
                        id_przesylki = c.Int(nullable: false),
                        Klientid_klienta = c.String(nullable: false, maxLength: 128),
                        Platnoscrodzaj = c.String(nullable: false, maxLength: 30),
                        Przesylkaid_przesylki = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_zamowienia);
            
            CreateTable(
                "dbo.ZamowionyTowar2",
                c => new
                    {
                        ilosc_sztuk = c.Int(nullable: false),
                        Towarid_towaru = c.Int(nullable: false),
                        Zamowienieid_zamowienia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ilosc_sztuk, t.Towarid_towaru, t.Zamowienieid_zamowienia });
            
            CreateTable(
                "dbo.ZamowionyTowar",
                c => new
                    {
                        ilosc_sztuk = c.Int(nullable: false),
                        Towarid_towaru = c.Int(nullable: false),
                        Zamowienieid_zamowienia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ilosc_sztuk, t.Towarid_towaru, t.Zamowienieid_zamowienia });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PosiadaKategorie", "Kategoriaid_kategorii", "dbo.Kategoria");
            DropForeignKey("dbo.Kategoria", "Kategoriaid_kategorii", "dbo.Kategoria");
            DropForeignKey("dbo.Przesylka", "Adresprzesylkiid_adresu", "dbo.Adresprzesylki");
            DropIndex("dbo.PosiadaKategorie", new[] { "Kategoriaid_kategorii" });
            DropIndex("dbo.Kategoria", new[] { "Kategoriaid_kategorii" });
            DropIndex("dbo.Przesylka", new[] { "Adresprzesylkiid_adresu" });
            DropTable("dbo.ZamowionyTowar");
            DropTable("dbo.ZamowionyTowar2");
            DropTable("dbo.Zamowienie");
            DropTable("dbo.Towar");
            DropTable("dbo.RodzajPlatnosci");
            DropTable("dbo.Rachunek");
            DropTable("dbo.Kurier");
            DropTable("dbo.Koszyk");
            DropTable("dbo.PosiadaKategorie");
            DropTable("dbo.Kategoria");
            DropTable("dbo.[dbo.RodzajPrzesylki]");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Przesylka");
            DropTable("dbo.Adresprzesylki");
        }
    }
}
