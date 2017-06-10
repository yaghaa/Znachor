using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Znachor.ViewModels;

namespace Znachor.Controllers
{
    public class OrderController : Controller
    {
      private Models.Znachor ctx = new Models.Znachor();

    // GET: Order
    public ActionResult Index()
        {
            var model = new Znachor.ViewModels.OrderViewModel();
            return View(model);
        }

      [HttpPost]
      public ActionResult Index(OrderViewModel model)
      {
      if(!ModelState.IsValid)
        {
        return View(model);
      }
        var userId = User.Identity.GetUserId();
        //var cartContent = ctx.Koszyks.Where(x => x.AspNetUsersid == userId);
      var zamowienie = new Znachor.Models.Zamowienie
      {
        Klientid_klienta = userId,
        Platnoscrodzaj = model.Payment,
        Przesylkaid_przesylki = 0,
        nr_rachunku = "123",
      };
        ctx.Zamowienies.Add(zamowienie);
        //ctx.SaveChanges();

        //var zamowienieId = ctx.Zamowienies.FirstOrDefault(x => x.Klientid_klienta == userId);
        var address = new Znachor.Models.Adresprzesylki
        {
          ulica = model.Street,
          kodpocztowy = model.PostalCode,
          miejscowosc = model.City,
          nrdomu = 1,
        };
        ctx.Adresprzesylkis.Add(address);
        ctx.SaveChanges();

        //TODO: Zapis do bazy
      return RedirectToAction("Index", "OderDetails");
     }
    }
}