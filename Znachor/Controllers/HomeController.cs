using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Znachor.Controllers
{
  public class HomeController : Controller
  {
    
    public ActionResult Index()
    {
      if (User.Identity.IsAuthenticated)
      {
        var userCookie = new HttpCookie("ShoppingCart", Helpers.Helpers.GetCartValue(User.Identity.GetUserId()));
        userCookie.Expires.AddDays(365);
        HttpContext.Response.Cookies.Add(userCookie);
      }
      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    } 
  }
}