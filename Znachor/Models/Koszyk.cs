namespace Znachor.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Koszyk")]
  public class Koszyk
  {
    [Key]
    [Column(Order = 0)]
    public string AspNetUsersid { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Towarid_towaru { get; set; }

    [Column(Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ilosc_sztuk { get; set; }
  }
}