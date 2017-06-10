using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Znachor.Controllers;
using Znachor.Models;

namespace Znachor.UnitTest
{
  [TestFixture]
  class ProductsControllerTests
  {
    private Mock<Models.Znachor> _ctx;
    private Mock<HttpRequestBase> _request;
    private Mock<HttpResponseBase> _response;
    private Mock<HttpContextBase> _context;
    private RequestContext _rc;
    private readonly string _userId = "test";

    private ProductsController _sut;

   [SetUp]
   public void SetUp()
    {
      _ctx = new Mock<Models.Znachor>();
      _request = new Mock<HttpRequestBase>(MockBehavior.Strict);
      _response = new Mock<HttpResponseBase>(MockBehavior.Strict);
      _context = new Mock<HttpContextBase>(MockBehavior.Strict);

      _sut = new ProductsController(_ctx.Object)
      {
        GetUserId = () => _userId
      };

      _request.SetupGet(x => x.ApplicationPath).Returns("/");
      _request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
      _request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

      _response.Setup(x => x.ApplyAppPathModifier(Moq.It.IsAny<String>())).Returns((String url) => url);
      
      _context.SetupGet(x => x.Request).Returns(_request.Object);
      _context.SetupGet(x => x.Response).Returns(_response.Object);
      _context.Setup(x => x.Response.SetCookie(It.IsAny<HttpCookie>()));

      _rc = new RequestContext(_context.Object, new RouteData());
    }

    [Test]
    public void AddToCart_adds_product_to_cart_if_item_doesnt_exists_in_cart()
    {
      //Arrange
      var producId = 1;
      var koszyk = new Koszyk { Towarid_towaru = producId };
      var towar = new Towar { cena_netto = 10 };
      var list = new List<Koszyk> { koszyk };

      _ctx.Setup(x => x.GetKoszyk(It.IsAny<int>(), It.IsAny<string>()));
      _ctx.Setup(x => x.GetKoszykWhere(It.IsAny<string>())).Returns(list.AsQueryable());
      _ctx.Setup(x => x.GetFirstTowar(producId)).Returns(towar);
      _ctx.Setup(x => x.SaveChanges()).Verifiable();
      _ctx.Setup(x => x.Koszyks.Add(It.IsAny<Models.Koszyk>())).Verifiable();
      
      _sut.ControllerContext = new ControllerContext(_rc, _sut);
      //Act
      _sut.AddToCart(producId);

      //ASSERT
      _ctx.Verify(x => x.SaveChanges(), Times.Once);
      _ctx.Verify(x => x.Koszyks.Add(It.IsAny<Models.Koszyk>()), Times.Once);
      _context.Verify(x => x.Response.SetCookie(It.IsAny<HttpCookie>()), Times.Once);

    }

    [Test]
    public void AddToCart_adds_product_to_cart_if_item_exists_in_cart()
    {
      //Arrange
      var id = 1;
      var idT = 1;
      var koszyk = new Models.Koszyk()
      {
        Towarid_towaru = idT
      };
      var towar = new Models.Towar()
      {
        cena_netto = 10
      };
      var list = new List<Koszyk>() { koszyk };

      _ctx.Setup(x => x.GetKoszyk(It.IsAny<int>(), It.IsAny<string>())).Returns(koszyk);
      _ctx.Setup(x => x.GetKoszykWhere(It.IsAny<string>())).Returns(list.AsQueryable());
      _ctx.Setup(x => x.GetFirstTowar(It.IsAny<int>())).Returns(towar);
      _ctx.Setup(x => x.SaveChanges());
      _ctx.Setup(x => x.Koszyks.Add(It.IsAny<Models.Koszyk>()));

      _sut.ControllerContext = new ControllerContext(_rc, _sut);
      //Act

      _sut.AddToCart(id);

      //Assert
      _ctx.Verify(x => x.SaveChanges(), Times.Once);
      _ctx.Verify(x => x.Koszyks.Add(It.IsAny<Models.Koszyk>()), Times.Never);
      _context.Verify(x => x.Response.SetCookie(It.IsAny<HttpCookie>()), Times.Once);
    }

    [Test]
    public void GetCartValue_returns_zero_when_no_items_in_cart()
    {
      //Arrange
      var list = new List<Koszyk>();
      _ctx.Setup(x => x.GetKoszykWhere(It.IsAny<string>())).Returns(list.AsQueryable());
      //Act
      var result =_sut.GetCartValue(_userId);
      //Assert
      Assert.AreEqual("0",result);
      _ctx.Verify(x => x.GetFirstTowar(It.IsAny<int>()), Times.Never);
      _ctx.Verify(x => x.GetKoszykWhere(_userId), Times.Once);
    }

    [Test]
    public void GetCartValue_returns_value_when_items_in_cart_exists()
    {
      //Arrange
      var idT = 1;
      var koszyk = new Models.Koszyk()
      {
        Towarid_towaru = idT,
        ilosc_sztuk = 2
      };
      var towar = new Models.Towar()
      {
        cena_netto = 10
      };
      var list = new List<Koszyk>(){ koszyk };

      _ctx.Setup(x => x.GetFirstTowar(It.IsAny<int>())).Returns(towar);
      _ctx.Setup(x => x.GetKoszykWhere(It.IsAny<string>())).Returns(list.AsQueryable());
      //Act
      var result = _sut.GetCartValue(_userId);
      //Assert
      Assert.AreEqual("20", result);
      _ctx.Verify(x => x.GetFirstTowar(idT), Times.Once);
      _ctx.Verify(x => x.GetKoszykWhere(_userId), Times.Once);
    }
  }
}
