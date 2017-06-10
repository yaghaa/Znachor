using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Znachor.ViewModels
{
  public class OrderViewModel
  {
    [Required]
    public string Shipment  { get; set; }
    [Required]
    public string Payment { get; set; }
    [Display(Name = "Imię")]
    [Required]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Nazwisko")]
    public string Surname { get; set; }
    [Required]
    [Display(Name = "Ulica")]
    public string Street { get; set; }
    [Required]
    [Display(Name = "Kod pocztowy")]
    public string PostalCode { get; set; }
    [Required]
    [Display(Name = "Miasto")]
    public string City { get; set; }
  }
}