//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security;
//using System.ServiceModel;
//using System.Web.Http;
//using Blake.Web.Controllers;
//using Blake.Web.Domains;
//using Blake.Web.Domains.Exceptions;
//using Blake.Web.Domains.Models;
//using Blake.Web.Domains.Models.ClientManagementFromFile;
//using KRD.WcfProxy.Faults.FromKRDServices;
//using log4net;
//using log4net.Config;
//using Moq;
//using NUnit.Framework;
//using Ploeh.AutoFixture;
//using Ploeh.AutoFixture.AutoMoq;
//using Ploeh.AutoFixture.Idioms;
//using Ploeh.AutoFixture.Kernel;
//using Shouldly;

//namespace Blake.Web.Tests.Controllers
//{
//  [TestFixture]
//  public class ParseFileControllerTest
//  {
//    private ParseFileController _sut;

//    private IFixture _mockFixture;

//    private Mock<ICsvProvider> _csvProviderMock;
//    private Mock<IConfigurationManager> _configurationManagerMock;
//    private Mock<IRahlQuery> _rahlQuerMock;
//    private Mock<IHttpInputStreamReader> _httpInputStremReader;

//    [TestFixtureSetUp]
//    public void TestFixtureSetup()
//    {
//      GlobalContext.Properties["applicationName"] = "Blake.Web.Tests.Controllers";
//      XmlConfigurator.Configure();
//    }

//    [SetUp]
//    public void Setup()
//    {
//      _mockFixture = new Fixture().Customize(new AutoMoqCustomization());

//      _mockFixture.Customize<ParseFileController>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())).OmitAutoProperties());

//      _csvProviderMock = _mockFixture.Freeze<Mock<ICsvProvider>>();
//      _configurationManagerMock = _mockFixture.Freeze<Mock<IConfigurationManager>>();
//      _rahlQuerMock = _mockFixture.Freeze<Mock<IRahlQuery>>();
//      _httpInputStremReader = _mockFixture.Freeze<Mock<IHttpInputStreamReader>>();

//      _sut = _mockFixture.Create<ParseFileController>();
//    }

//    [Test]
//    public void Ctor_checks_its_arguments()
//    {
//      // arrange
//      var assertion = new GuardClauseAssertion(_mockFixture);

//      // act assert
//      assertion.Verify(typeof(ParseFileController).GetConstructors());
//    }

//    [Test(Description = "XTC-2401")]
//    public void Post_file_not_exists()
//    {
//      // assign
//      _httpInputStremReader.Setup(c => c.GetRequestFileCount()).Returns(0);

//      // act assert
//      Assert.Throws<HttpResponseException>(() => _sut.Post());
//    }

//    [TestCase(2)]
//    [TestCase(3)]
//    [TestCase(4)]
//    [TestCase(5)]
//    public void Post_selected_multiple_files(int fileCount)
//    {
//      // assign
//      _httpInputStremReader.Setup(c => c.GetRequestFileCount()).Returns(fileCount);

//      // act assert
//      Assert.Throws<HttpResponseException>(() => _sut.Post());
//    }

//    [Test(Description = "XTC-2392")]
//    public void Post_selected_single_file()
//    {
//      // arrange
//      var errorList = new List<Error>();
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      var readFile = new Fixture().Create<ReadFile>();
//      _csvProviderMock.Setup(p => p.GetData(It.IsAny<Stream>(), It.IsAny<string>(), out errorList)).Returns(readFile);

//      // act & assert
//      Assert.DoesNotThrow(() => _sut.Post());
//      _rahlQuerMock.Verify(x => x.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>()), Times.Once());
//    }

//    [Test]
//    public void Post_httpInputStremReader_throws_from_ReadInputStream()
//    {
//      // arrange
//      string filename;
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);
//      _httpInputStremReader.Setup(r => r.ReadInputStream(out filename)).Throws<FileLoadException>();

//      // act & assert
//      Assert.Throws<HttpResponseException>(() => _sut.Post());
//    }

//    [Test]
//    public void Post_httpInputStremReader_throws_from_SavePostedFile()
//    {
//      // arrange
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      // act & assert
//      Assert.Throws<HttpResponseException>(() => _sut.Post());
//    }

//    [Test]
//    public void Post_csvProvider_throws_FileHasErrorsException()
//    {
//      // arrange
//      List<Error> errorList;
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      _csvProviderMock.Setup(p => p.GetData(It.IsAny<Stream>(), It.IsAny<string>(), out errorList)).Throws(new FileHasErrorsException("MyErrorMessage"));

//      // act & assert
//      Assert.That(() => _sut.Post(), Throws.InstanceOf<HttpResponseException>());
//    }

//    [Test]
//    public void Post_rahlProviderMock_throws_SecurityFault()
//    {
//      // arrange
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      var faultException = new FaultException<SecurityFault>(It.IsAny<SecurityFault>());
//      _rahlQuerMock.Setup(p => p.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>())).Throws(faultException);

//      // act & assert
//      Assert.That(() => _sut.Post(), Throws.InstanceOf<HttpResponseException>());
//    }

//    [Test]
//    public void Post_rahlProviderMock_throws_SecurityException()
//    {
//      // arrange
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      _rahlQuerMock.Setup(p => p.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>())).Throws<SecurityException>();

//      // act & assert
//      Assert.That(() => _sut.Post(), Throws.InstanceOf<HttpResponseException>());
//    }

//    [Test(Description = "XTC-2388")]
//    public void Post_csvProvider_returns_clientRow_with_nullable_Guid()
//    {
//      // arrange
//      List<Error> errorList;
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);
//      var readFile = new Fixture().Create<ReadFile>();
//      readFile.ClientRows[0].ClientId = null;
//      _csvProviderMock.Setup(p => p.GetData(It.IsAny<Stream>(), It.IsAny<string>(), out errorList)).Returns(readFile);

//      // act & assert
//      Assert.That(() => _sut.Post(), Throws.InstanceOf<HttpResponseException>());
//      _rahlQuerMock.Verify(x => x.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>()), Times.Once());
//    }

//    [Test(Description = "XTC-2388")]
//    public void Post_csvProvider_returns_clientRow_with_empty_NIP()
//    {
//      // arrange
//      List<Error> errorList;
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      var readFile = new Fixture().Create<ReadFile>();
//      readFile.ClientRows[0].NIP = string.Empty;
//      _csvProviderMock.Setup(p => p.GetData(It.IsAny<Stream>(), It.IsAny<string>(), out errorList)).Returns(readFile);

//      // act & assert
//      Assert.That(() => _sut.Post(), Throws.InstanceOf<HttpResponseException>());
//      _rahlQuerMock.Verify(x => x.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>()), Times.Once());
//    }

//    [Test(Description = "XTC-2392")]
//    public void Post_fills()
//    {
//      // arrange
//      var crmId = "CRM/id1234";
//      var errorList = new List<Error>();
//      _httpInputStremReader.Setup(r => r.GetRequestFileCount()).Returns(1);

//      var readFile = new ReadFile { ClientRows = new List<ClientRow> { new ClientRow() { CrmId = crmId } } };
//      _csvProviderMock.Setup(p => p.GetData(It.IsAny<Stream>(), It.IsAny<string>(), out errorList)).Returns(readFile);

//      var rahlClientData = new List<RahlClientData> { new RahlClientData { CrmId = crmId, TaxId = "123456789", DisplayName = "rahlClientName" } };
//      _rahlQuerMock.Setup(q => q.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>())).Returns(rahlClientData);

//      // act & assert
//      Assert.DoesNotThrow(() => _sut.Post());
//      readFile.ClientRows.First().NIP.ShouldBe(rahlClientData.First().TaxId);
//      readFile.ClientRows.First().Name.ShouldBe(rahlClientData.First().DisplayName);
//      readFile.ClientRows.First().ClientId.ShouldBe(rahlClientData.First().ClientId);
//      _rahlQuerMock.Verify(x => x.GetRahlClientDataByCrmIds(It.IsAny<IEnumerable<string>>()), Times.Once());
//    }
//  }
//}