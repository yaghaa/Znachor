using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Znachor.ViewModels;

namespace Znachor.Controllers
{
    public class OrderController : Controller
    {
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
       
      //TODO: Zapis do bazy
      return RedirectToAction("Index", "OderDetails");
     }
    }
}