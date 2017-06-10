namespace Znachor.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class asd : DbMigration
  {
    public override void Up()
    {
      AddPrimaryKey("dbo.Koszyk", new[] { "AspNetUsersid", "Towarid_towaru" });
    }
  }
}