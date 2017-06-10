using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Znachor.Controllers
{
  public class ProductsController : Controller
  {
    private Models.Znachor _ctx;
    public Func<string> GetUserId;

    public ProductsController(Models.Znachor ctx)
    {
      GetUserId = () => User.Identity.GetUserId();
      if (ctx != null)
      {
        _ctx = ctx;
      }
      else
      {
        _ctx = new Models.Znachor();
      }
    }
    // GET: Products
    public ActionResult Index()
    {
      var products = _ctx.Towars;
      return View(products.ToList());
    }

    public RedirectToRouteResult AddToCart(int id)
    {
      var a = _ctx.GetKoszyk(id, GetUserId());

      if (a == null)
      {
        var koszyk = new Models.Koszyk()
        {
          AspNetUsersid = GetUserId(),
          Towarid_towaru = id,
          ilosc_sztuk = 1
        };
        _ctx.Koszyks.Add(koszyk);
      }
      else
      {
        a.ilosc_sztuk += 1;
      }

      _ctx.SaveChanges();
      var userCookie = new System.Web.HttpCookie("ShoppingCart",
        GetCartValue(GetUserId()));
      HttpContext.Response.SetCookie(userCookie);
      return RedirectToAction("Index");
    }


    public string GetCartValue(string id)
    {
      decimal sum = 0;
      var values = _ctx.GetKoszykWhere(id);

      if (values.Any())
      {
        foreach (var v in values)
        {
          var towar = _ctx.GetFirstTowar(v.Towarid_towaru);
          sum += v.ilosc_sztuk * towar.cena_netto;
        }
      }
      return sum.ToString();
    }
  }
}