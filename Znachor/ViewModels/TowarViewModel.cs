using System.ComponentModel.DataAnnotations;

namespace Znachor.ViewModels
{
  public partial class TowarViewModel
  {
    public int count { get; set; }

    public int id_towaru { get; set; }

    [Required]
    [StringLength(50)]
    public string nazwa { get; set; }

    public decimal cena_netto { get; set; }

    public int ilosc_w_magazynie { get; set; }

    [Required]
    [StringLength(40)]
    public string producent { get; set; }

    [Required]
    [StringLength(25)]
    public string forma { get; set; }

    [Required]
    [StringLength(255)]
    public string sklad { get; set; }

    [Required]
    [StringLength(255)]
    public string szczegoly { get; set; }
  }
}