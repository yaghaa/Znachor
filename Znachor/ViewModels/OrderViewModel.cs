using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Znachor.ViewModels
{
  public class OrderViewModel
  {
    public string Shipment  { get; set; }
    public string Payment { get; set; }
    [Display(Name = "Imię")]
    public string FirstName { get; set; }
    [Display(Name = "Nazwisko")]
    public string Surname { get; set; }
    [Display(Name = "Ulica")]
    public string Street { get; set; }
    [Display(Name = "Kod pocztowy")]
    public string PostalCode { get; set; }
    [Display(Name = "Miasto")]
    public string City { get; set; }
  }
}