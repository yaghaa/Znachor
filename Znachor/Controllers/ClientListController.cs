//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Reflection;
//using System.Security;
//using System.ServiceModel;
//using System.ServiceModel.Security;
//using System.Web.Http;
//using Blake.Web.Domains;
//using Blake.Web.DTO;
//using Blake.Web.DTO.ClientListController;
//using Jackson.Proxy.Dto.Entities.InternalClientDataQuery;
//using Jackson.Proxy.Dto.Entities.InternalNfgClientCommand;
//using KRD.Common.SyntaxSugar;
//using KRD.Logging.Extensions;
//using KRD.WcfProxy.Faults.FromKRDServices;
//using log4net;

//namespace Blake.Web.Controllers
//{
//  public class ClientListController : ApiController
//  {
//    private readonly IRahlQuery _rahlQueryFactory;
//    private readonly IJacksonQuery _jacksonQuery;
//    private readonly IJacksonCommand _jacksonCommand;

//    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

//    public ClientListController(IRahlQuery rahlQueryFactory, IJacksonQuery jacksonQuery, IJacksonCommand jacksonCommand)
//    {
//      Guard.NotNull(() => rahlQueryFactory, () => rahlQueryFactory);
//      Guard.NotNull(() => jacksonQuery, () => jacksonQuery);
//      Guard.NotNull(() => jacksonCommand, () => jacksonCommand);

//      _rahlQueryFactory = rahlQueryFactory;
//      _jacksonQuery = jacksonQuery;
//      _jacksonCommand = jacksonCommand;
//    }

//    // GET: api/ClientList
//    public GetResult Get([FromUri] GetRequest getRequestParams = null)
//    {
//      _log.DebugIfIsEnabled("Get() invoked");

//      try
//      {
//        GetNfgClientsQueryResponse clientsQueryResponse = _jacksonQuery.GetNfgClientsQuery(new GetNfgClientsQuery());
//        List<Guid> clientIds = clientsQueryResponse.NfgClients.Select(x => x.ClientId).ToList();
//        var clientInfos = new List<ClientInfo>();

//        foreach (var crmItem in _rahlQueryFactory.GetRahlClientDataByClientIds(clientIds))
//        {
//          var jacksonData = clientsQueryResponse.NfgClients.First(c => c.ClientId == crmItem.ClientId);

//          clientInfos.Add(new ClientInfo()
//          {
//            ClientId = jacksonData.ClientId,
//            FaktoringHasContract = jacksonData.FactoringInfo.HasContract,
//            FaktoringIsPreselect = jacksonData.FactoringInfo.IsPreselect,
//            FaktoringIsPromotionClient = jacksonData.FactoringInfo.IsPromotionClient,
//            LoanHasContract = jacksonData.LoanInfo.HasContract,
//            LoanIsPreselect = jacksonData.LoanInfo.IsPreselect,
//            LoanIsPromotionClient = jacksonData.LoanInfo.IsPromotionClient,
//            SelectedToSeeSpecialBanner = jacksonData.SelectedToSeeSpecialBanner,

//            CrmId = crmItem.CrmId,
//            DisplayName = crmItem.DisplayName,
//            TaxId = crmItem.TaxId
//          });
//        }

//        _log.DebugIfIsEnabled("Get() done");

//        return new GetResult { ClientInfos = clientInfos, ItemsToGet = clientsQueryResponse.NfgClients.Count };
//      }
//      catch (FaultException<ValidationFault> exception)
//      {
//        _log.WarnIfIsEnabled("There is issue with validation", exception);
//        throw new HttpResponseException(HttpStatusCode.BadRequest);
//      }
//      catch (Exception exception)
//        when (exception is FaultException<SecurityFault> || exception is SecurityAccessDeniedException)
//      {
//        _log.WarnIfIsEnabled("There is issue with security", exception);
//        throw new HttpResponseException(HttpStatusCode.Forbidden);
//      }
//      catch (Exception exception)
//      {
//        _log.WarnIfIsEnabled("Something went wrong", exception);
//        throw new HttpResponseException(HttpStatusCode.InternalServerError);
//      }
//    }

//    [Route("api/ClientList/UpdateClients")]
//    public void UpdateClients([FromBody] PostRequest postRequest)
//    {
//      if (postRequest?.ClientsToSave == null)
//      {
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "Request body should not be null"
//        });
//      }

//      if (postRequest.ClientsToSave.Count < 1)
//      {
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "ClientsToSave list should contain at least one element"
//        });
//      }

//      try
//      {
//        _jacksonCommand.UpdateClients(new UpdateClientsCommand()
//        {
//          ClientsMarketingData = postRequest.ClientsToSave.Select(info => new ClientMarketingData()
//          {
//            ClientId = info.ClientId,
//            FactoringData = new FactoringData
//            {
//              Preselect = info.FaktoringIsPreselect,
//              SpecialOffer = info.FaktoringIsPromotionClient
//            },
//            LoanData = new LoanData
//            {
//              Preselect = info.LoanIsPreselect,
//              SpecialOffer = info.LoanIsPromotionClient
//            },
//            SelectedToSeeSpecialBanner = info.SelectedToSeeSpecialBanner
//          }).ToList()
//        });
//      }
//      catch (FaultException<ValidationFault> validationFault)
//      {
//        _log.WarnIfIsEnabled("There was an issue with validation", validationFault);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "There was an issue with validation"
//        });
//      }
//      catch (Exception exception) when (exception is FaultException<SecurityFault> || exception is SecurityException)
//      {
//        _log.WarnIfIsEnabled("There was an issue with security", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
//      }
//      catch (Exception exception)
//      {
//        _log.ErrorIfIsEnabled("Something went wrong", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
//      }
//    }

//    public ClientsRows Put([FromBody] ClientCrmIds clientCrmIds)
//    {
//      try
//      {
//        return _jacksonQuery.GetClients(clientCrmIds.Ids);
//      }
//      catch (FaultException<ValidationFault> validationFault)
//      {
//        _log.WarnIfIsEnabled("There was an issue with validation", validationFault);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "There was an issue with validation"
//        });
//      }
//      catch (Exception exception) when (exception is FaultException<SecurityFault> || exception is SecurityException)
//      {
//        _log.WarnIfIsEnabled("There was an issue with security", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
//      }
//      catch (Exception exception)
//      {
//        _log.ErrorIfIsEnabled("Something went wrong", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
//      }
//    }

//    [Route("api/ClientList/AddOrSkipClients")]
//    public void AddOrSkipClients([FromBody] PostRequest postRequest)
//    {
//      if (postRequest?.ClientsToSave == null)
//      {
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "Request body should not be null"
//        });
//      }

//      if (postRequest.ClientsToSave.Count < 1)
//      {
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "ClientsToSave list should contain at least one element"
//        });
//      }

//      try
//      {
//        _jacksonCommand.AddOrSkipClients(new AddOrSkipClientsCommand
//        {
//          AddClientRequests = postRequest.ClientsToSave.Select(info => new ClientMarketingData
//          {
//            ClientId = info.ClientId,
//            FactoringData = new FactoringData
//            {
//              Preselect = info.FaktoringIsPreselect,
//              SpecialOffer = info.FaktoringIsPromotionClient
//            },
//            LoanData = new LoanData
//            {
//              Preselect = info.LoanIsPreselect,
//              SpecialOffer = info.LoanIsPromotionClient
//            },
//            SelectedToSeeSpecialBanner = info.SelectedToSeeSpecialBanner
//          }).ToList()
//        });
//      }
//      catch (FaultException<ValidationFault> validationFault)
//      {
//        _log.WarnIfIsEnabled("There was an issue with validation", validationFault);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          ReasonPhrase = "There was an issue with validation"
//        });
//      }
//      catch (Exception exception) when (exception is FaultException<SecurityFault> || exception is SecurityException)
//      {
//        _log.WarnIfIsEnabled("There was an issue with security", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
//      }
//      catch (Exception exception)
//      {
//        _log.ErrorIfIsEnabled("Something went wrong", exception);
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
//      }
//    }
//  }
//}