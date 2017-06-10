//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Security;
//using System.ServiceModel;
//using System.Web.Http;
//using Blake.Web.Controllers;
//using Blake.Web.Domains;
//using Blake.Web.Domains.Models;
//using Blake.Web.DTO;
//using Blake.Web.DTO.ClientListController;
//using Jackson.Proxy.Dto.Entities.InternalClientDataQuery;
//using Jackson.Proxy.Dto.Entities.InternalNfgClientCommand;
//using KRD.WcfProxy.Faults.FromKRDServices;
//using log4net;
//using log4net.Config;
//using Moq;
//using NUnit.Framework;
//using Ploeh.AutoFixture;
//using Shouldly;

//namespace Blake.Web.Tests.Controllers
//{
//  [TestFixture]
//  public class ClientListControllerTests
//  {
//    private ClientListController _sut;
//    private IFixture _mockFixture;
//    private MockRepository _mockRepository = new MockRepository(MockBehavior.Default);
//    private Mock<IRahlQuery> _mockQueryFactory;
//    private Mock<IJacksonQuery> _mockJacksonQuery;
//    private Mock<IJacksonCommand> _mockJacksonCommand;

//    [TestFixtureSetUp]
//    public void TestFixtureSetup()
//    {
//      GlobalContext.Properties["applicationName"] = "Blake.Web.Tests.Controllers";
//      XmlConfigurator.Configure();
//    }

//    [SetUp]
//    public void Setup()
//    {
//      _mockFixture = new Fixture();
//      _mockFixture.Register(() => true);
//      _mockQueryFactory = _mockRepository.Create<IRahlQuery>();
//      _mockJacksonQuery = _mockRepository.Create<IJacksonQuery>();
//      _mockJacksonCommand = _mockRepository.Create<IJacksonCommand>();
//      _sut = new ClientListController(_mockQueryFactory.Object, _mockJacksonQuery.Object, _mockJacksonCommand.Object);
//    }

//    #region Get

//    [Test]
//    public void Get_throws_403_on_SecurityFault()
//    {
//      // assign
//      _mockJacksonQuery.Setup(query => query.GetNfgClientsQuery(It.IsAny<GetNfgClientsQuery>()))
//        .Throws(new FaultException<SecurityFault>(new SecurityFault()));

//      // act
//      var act = new Action(() => _sut.Get());

//      // assert
//      act.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//    }

//    [Test]
//    public void Get_throws_400_on_ValidationFault()
//    {
//      // assign
//      _mockJacksonQuery.Setup(query => query.GetNfgClientsQuery(It.IsAny<GetNfgClientsQuery>()))
//        .Throws(new FaultException<ValidationFault>(new ValidationFault()));

//      // act
//      var act = new Action(() => _sut.Get());

//      // assert
//      act.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//    }

//    [Test]
//    public void Get_throws_500_on_UnhandledException()
//    {
//      // assign
//      _mockJacksonQuery.Setup(query => query.GetNfgClientsQuery(It.IsAny<GetNfgClientsQuery>()))
//        .Throws(new Exception());

//      // act
//      var act = new Action(() => _sut.Get());

//      // assert
//      act.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
//    }

//    [Test]
//    public void Get_returns_same_count_of_clients_from_Jackson_asce()
//    {
//      // assign
//      var mockedClients = _mockFixture.CreateMany<KeyValuePair<RahlClientData, NfgClientData>>(1000).ToList();
//      foreach (var mockedClient in mockedClients)
//      {
//        mockedClient.Key.ClientId = mockedClient.Value.ClientId = Guid.NewGuid();
//      }

//      _mockJacksonQuery.Setup(query => query.GetNfgClientsQuery(It.IsAny<GetNfgClientsQuery>()))
//        .Returns(() => new GetNfgClientsQueryResponse()
//        {
//          NfgClients = mockedClients.Select(kvp => kvp.Value).ToList()
//        });

//      var odataMockData = mockedClients.Select(kvp => kvp.Key).ToList().AsQueryable();

//      _mockQueryFactory
//      .Setup(r => r.GetRahlClientDataByClientIds(It.IsAny<IEnumerable<Guid>>()))
//      .Returns(() => odataMockData.AsQueryable());

//      var clientIdsSorted = mockedClients.Select(kvp => kvp.Key);

//      var clientIdsSortedList = clientIdsSorted.Select(data => data.ClientId).ToList();

//      // act
//      var request = GetRequest.Default;

//      var clientInfos = _sut.Get(request);

//      // assert
//      clientInfos.ClientInfos.Count.ShouldBe(mockedClients.Count);
//      clientInfos.ClientInfos.Select(info => info.ClientId).Zip(clientIdsSortedList, (cid1, cid2) => cid1 == cid2).ShouldAllBe(b => b);
//      clientInfos.ItemsToGet.ShouldBe(mockedClients.Count);
//    }

//    #endregion Get

//    #region UpdateClients

//    [Test]
//    public void Post_should_throw_400_on_null_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.UpdateClients(null);

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("Request body should not be null")
//        );
//    }

//    [Test]
//    public void Post_should_throw_400_on_null_list_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = null });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("Request body should not be null")
//        );
//    }

//    [Test]
//    public void Post_should_throw_400_on_empty_list_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = new List<ClientInfo>() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("ClientsToSave list should contain at least one element")
//        );
//    }

//    [Test]
//    public void Post_should_throw_403_on_security_exception()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.UpdateClients(It.IsAny<UpdateClientsCommand>()))
//        .Throws<SecurityException>();

//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.Forbidden)
//        );
//    }

//    [Test]
//    public void Post_should_throw_403_on_security_fault()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.UpdateClients(It.IsAny<UpdateClientsCommand>()))
//        .Throws(new FaultException<SecurityFault>(new SecurityFault()));

//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.Forbidden)
//        );
//    }

//    [Test]
//    public void Post_should_throw_400_on_validation_fault()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.UpdateClients(It.IsAny<UpdateClientsCommand>()))
//        .Throws(new FaultException<ValidationFault>(new ValidationFault()));

//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
//        );
//    }

//    [Test]
//    public void Post_should_throw_500_on_UnhandledException()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.UpdateClients(It.IsAny<UpdateClientsCommand>()))
//        .Throws(new Exception());

//      //act
//      Action pushAct = () => _sut.UpdateClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      pushAct.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
//    }

//    [Test]
//    public void Post_not_throw_exception()
//    {
//      //arrange
//      var clientsList = new PostRequest
//      {
//        ClientsToSave = _mockFixture.CreateMany<ClientInfo>(10).ToList()
//      };

//      //act
//      Action pushAct = () => _sut.UpdateClients(clientsList);

//      //assert
//      pushAct.ShouldNotThrow();
//      _mockJacksonCommand.Verify(
//        command =>
//          command.UpdateClients(
//            It.Is<UpdateClientsCommand>(
//              updateCmd =>
//                updateCmd.ClientsMarketingData.TrueForAll(
//                  data =>
//                    clientsList.ClientsToSave.SingleOrDefault(
//                      item =>
//                        item.ClientId == data.ClientId &&
//                        item.FaktoringIsPreselect == data.FactoringData.Preselect &&
//                        item.FaktoringIsPromotionClient == data.FactoringData.SpecialOffer &&
//                        item.LoanIsPreselect == data.LoanData.Preselect &&
//                        item.LoanIsPromotionClient == data.LoanData.SpecialOffer &&
//                        item.SelectedToSeeSpecialBanner == data.SelectedToSeeSpecialBanner) != null))));
//    }

//    #endregion UpdateClients

//    #region Put

//    [Test]
//    public void PostFilteredClientList_should_throw_400_on_validation_fault()
//    {
//      // assign
//      var clientCrmIds = _mockFixture.Create<ClientCrmIds>();
//      _mockJacksonQuery.Setup(command => command.GetClients(It.IsAny<List<Guid>>()))
//        .Throws(new FaultException<ValidationFault>(new ValidationFault()));

//      //act
//      Action pushAct = () => _sut.Put(clientCrmIds);

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
//        );
//    }

//    [Test]
//    public void PostFilteredClientList_should_throw_403_on_security_fault()
//    {
//      //arrange
//      var clientCrmIds = _mockFixture.Create<ClientCrmIds>();
//      _mockJacksonQuery.Setup(command => command.GetClients(It.IsAny<List<Guid>>()))
//        .Throws(new FaultException<SecurityFault>(new SecurityFault()));

//      //act
//      Action pushAct = () => _sut.Put(clientCrmIds);

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.Forbidden)
//        );
//    }

//    [Test]
//    public void PostFilteredClientList_should_throw_500_on_UnhandledException()
//    {
//      // assign
//      var clientCrmIds = _mockFixture.Create<ClientCrmIds>();
//      _mockJacksonQuery.Setup(command => command.GetClients(It.IsAny<List<Guid>>()))
//        .Throws(new Exception());

//      // act
//      Action pushAct = () => _sut.Put(clientCrmIds);

//      // assert
//      pushAct.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
//    }

//    #endregion Put

//    #region AddOrSkipClients

//    [Test]
//    public void AddOrSkipClients_should_throw_400_on_null_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(null);

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("Request body should not be null")
//        );
//    }

//    [Test]
//    public void AddOrSkipClients_should_throw_400_on_null_list_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = null });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("Request body should not be null")
//        );
//    }

//    [Test]
//    public void AddOrSkipClients_should_throw_400_on_empty_list_request()
//    {
//      //arrange
//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = new List<ClientInfo>() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest),
//        () => response.ReasonPhrase.ShouldBe("ClientsToSave list should contain at least one element")
//        );
//    }

//    [Test]
//    public void AddOrSkipClients_should_throw_400_on_validation_fault()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.AddOrSkipClients(It.IsAny<AddOrSkipClientsCommand>()))
//        .Throws(new FaultException<ValidationFault>(new ValidationFault()));

//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
//        );
//    }

//    [Test]
//    public void AddOrSkipClients_should_throw_403_on_security_exception()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.AddOrSkipClients(It.IsAny<AddOrSkipClientsCommand>()))
//        .Throws<SecurityException>();

//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.Forbidden)
//        );
//    }

//    [Test]
//    public void AddOrSkipClientsCommand_should_throw_403_on_security_fault()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.AddOrSkipClients(It.IsAny<AddOrSkipClientsCommand>()))
//        .Throws(new FaultException<SecurityFault>(new SecurityFault()));

//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      var response = pushAct.ShouldThrow<HttpResponseException>().Response;
//      response.ShouldSatisfyAllConditions(
//        () => response.StatusCode.ShouldBe(HttpStatusCode.Forbidden)
//        );
//    }

//    [Test]
//    public void AddOrSkipClientsCommand_should_throw_500_on_UnhandledException()
//    {
//      //arrange
//      _mockJacksonCommand.Setup(command => command.AddOrSkipClients(It.IsAny<AddOrSkipClientsCommand>()))
//        .Throws(new Exception());

//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(new PostRequest() { ClientsToSave = _mockFixture.CreateMany<ClientInfo>().ToList() });

//      //assert
//      pushAct.ShouldThrow<HttpResponseException>().Response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
//    }

//    [Test]
//    public void AddOrSkipClientsCommand_not_throw_exception()
//    {
//      //arrange
//      var clientsList = new PostRequest
//      {
//        ClientsToSave = _mockFixture.CreateMany<ClientInfo>(10).ToList()
//      };

//      //act
//      Action pushAct = () => _sut.AddOrSkipClients(clientsList);

//      //assert
//      pushAct.ShouldNotThrow();
//      _mockJacksonCommand.Verify(
//        command =>
//          command.AddOrSkipClients(
//            It.Is<AddOrSkipClientsCommand>(
//              updateCmd =>
//                updateCmd.AddClientRequests.TrueForAll(
//                  data =>
//                    clientsList.ClientsToSave.SingleOrDefault(
//                      item =>
//                        item.ClientId == data.ClientId &&
//                        item.FaktoringIsPreselect == data.FactoringData.Preselect &&
//                        item.FaktoringIsPromotionClient == data.FactoringData.SpecialOffer &&
//                        item.LoanIsPreselect == data.LoanData.Preselect &&
//                        item.LoanIsPromotionClient == data.LoanData.SpecialOffer &&
//                        item.SelectedToSeeSpecialBanner == data.SelectedToSeeSpecialBanner) != null))));
//    }
//  }

//  #endregion AddOrSkipClients
//}