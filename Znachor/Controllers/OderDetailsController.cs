using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Znachor.Models;
using Znachor.ViewModels;

namespace Znachor.Controllers
{
    public class OderDetailsController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            var model = new OrderDetailsViewModel()
            {
              Payment = "testPayment",
              City = "testCity",
              FirstName = "testFN",
              PostalCode = "testCode",
              Shipment = "testShip",
              Street = "testStreet",
              Surname = "testSurnma",
              OrderedProducts = new List<OderedProductModel>() { new OderedProductModel() { Ilosc_sztuk = 1,Cena = 20,Towarid_towaru = 2,Nazwa = "testNazwy"} }
            };//TODO: pobranie modelu
            return View(model);
        }
    }
}