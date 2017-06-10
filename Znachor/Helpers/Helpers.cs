using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Znachor.Helpers
{
  public static class Helpers
  {
      public static string GetCartValue(string id)
      {
        var ctx = new Models.Znachor();
        decimal sum = 0;
        var values = ctx.Koszyks.Where(x => x.AspNetUsersid == id);

        if (values.Any())
        {
          foreach (var v in values)
          {
            var towar = ctx.Towars.First(x => x.id_towaru == v.Towarid_towaru);
            sum += v.ilosc_sztuk * towar.cena_netto;
          }
        }
        return sum.ToString();
      }    
  }
}