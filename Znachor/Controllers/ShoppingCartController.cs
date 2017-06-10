using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Znachor.Models;
using Znachor.ViewModels;

namespace Znachor.Controllers
{
  public class ShoppingCartController : Controller
  {
    private Models.Znachor ctx = new Models.Znachor();

    // GET: ShoppingCart
    public ActionResult Index()
    {
      var userId = User.Identity.GetUserId();
      var cartContent = ctx.Koszyks.Where(x => x.AspNetUsersid == userId);
      var products = new List<TowarViewModel>();

      foreach (var p in cartContent)
      {
        var product = ctx.Towars.First(towar => towar.id_towaru == p.Towarid_towaru);
        var productViewModel = Convert(product, p.ilosc_sztuk);
        if (productViewModel != null) products.Add(productViewModel);
      }

      return View(products);
    }

    public RedirectToRouteResult DeletoFromCart(int id)
    {
      var products = ctx.Koszyks.FirstOrDefault(x => x.Towarid_towaru == id);

      if (products != null)
      {
        ctx.Koszyks.Remove(products);
      }
      ctx.SaveChanges();
      var userCookie = new System.Web.HttpCookie("ShoppingCart", Helpers.Helpers.GetCartValue(User.Identity.GetUserId()));
      HttpContext.Response.SetCookie(userCookie);
      return RedirectToAction("Index");
    }

    public RedirectToRouteResult ChangeProductCount(int id, int i)
    {
      var products = ctx.Koszyks.FirstOrDefault(x => x.Towarid_towaru == id);

      if (products != null)
      {
        if(i == 0)
        {
          ctx.Koszyks.Remove(products);
        }
        else
        {
          products.ilosc_sztuk = i;
        }
      }
      ctx.SaveChanges();
      var userCookie = new System.Web.HttpCookie("ShoppingCart", Helpers.Helpers.GetCartValue(User.Identity.GetUserId()));
      HttpContext.Response.SetCookie(userCookie);
      return RedirectToAction("Index");
    }

    private TowarViewModel Convert(Towar towar, int count)
    {
      var model = new TowarViewModel()
      {
        id_towaru = towar.id_towaru,
        cena_netto = towar.cena_netto,
        forma = towar.forma,
        ilosc_w_magazynie = towar.ilosc_w_magazynie,
        nazwa = towar.nazwa,
        producent = towar.producent,
        sklad = towar.sklad,
        szczegoly = towar.szczegoly,
        count = count
      };
      return model;
    }
  }
}