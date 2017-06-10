using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Znachor.Models;

namespace Znachor.ViewModels
{
  public class OrderDetailsViewModel
  { 
    public string Shipment { get; set; }

    public string Payment { get; set; }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string Street { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public List<OderedProductModel> OrderedProducts { get; set; }

    public decimal Amount => CalculateAmount();

    private decimal CalculateAmount()
    {
      if (OrderedProducts.Any())
      {
        decimal value = 0;
        foreach (var product in OrderedProducts)
        {
          value += product.Ilosc_sztuk*product.Cena;
        }
        return value;
      }
      return 0;
    }

  }
}