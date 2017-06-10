//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Reflection;
//using System.Security;
//using System.ServiceModel;
//using System.Web.Http;
//using Blake.Web.Domains;
//using Blake.Web.Domains.Exceptions;
//using Blake.Web.Domains.Models.ClientManagementFromFile;
//using Blake.Web.Resources;
//using KRD.Common.SyntaxSugar;
//using KRD.Logging.Extensions;
//using KRD.WcfProxy.Faults.FromKRDServices;
//using log4net;
//using Newtonsoft.Json;

//namespace Blake.Web.Controllers
//{
//  public class ParseFileController : ApiController
//  {
//    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

//    private readonly ICsvProvider _csvProvider;

//    private readonly IRahlQuery _rahlQuery;

//    private readonly IHttpInputStreamReader _httpInputStreamReader;

//    private readonly string _multipleFilesSentMessage = Errors.MultipleFilesSent;

//    private readonly string _fileNotExistsMessage = Errors.FileNotExists;

//    public ParseFileController(ICsvProvider csvProvider, IRahlQuery rahlQuery, IHttpInputStreamReader httpInputStreamReader)
//    {
//      Guard.NotNull(() => csvProvider, () => csvProvider);
//      Guard.NotNull(() => rahlQuery, () => rahlQuery);
//      Guard.NotNull(() => httpInputStreamReader, () => httpInputStreamReader);

//      _csvProvider = csvProvider;
//      _rahlQuery = rahlQuery;
//      _httpInputStreamReader = httpInputStreamReader;
//    }

//    public ReadFile Post()
//    {
//      try
//      {
//        ReadFile readFile = null;
//        List<Error> errorList;
//        string fileName;

//        if (_httpInputStreamReader.GetRequestFileCount() == 0)
//        {
//          throw new FileLoadException(_fileNotExistsMessage);
//        }

//        if (_httpInputStreamReader.GetRequestFileCount() > 1)
//        {
//          throw new FileLoadException(_multipleFilesSentMessage);
//        }

//        var inputStream = _httpInputStreamReader.ReadInputStream(out fileName);
//        readFile = _csvProvider.GetData(inputStream, fileName, out errorList);
//        SetClientNipAndClientNameAndClientId(readFile);
//        errorList.AddRange(CheckExistsClientNip(readFile));

//        if (errorList.Any() == false)
//        {
//          return readFile;
//        }

//        var errors = JsonConvert.SerializeObject(errorList.OrderBy(e => e.RowNumber));
//        throw new FileHasErrorsException(errors);
//      }
//      catch (FileHasErrorsException exception)
//      {
//        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          Content = new StringContent(exception.Message)
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

//    private static List<Error> CheckExistsClientNip(ReadFile readFile)
//    {
//      if (readFile.ClientRows.Any(cr => string.IsNullOrEmpty(cr.NIP)))
//      {
//        var errors = readFile.ClientRows.Where(cr => string.IsNullOrEmpty(cr.NIP))
//          .Select(
//            notExistsClientRow =>
//              new Error()
//              {
//                Message = string.Format(Errors.ClientNotExists, notExistsClientRow.CrmId),
//                RowNumber = notExistsClientRow.RowNumber
//              })
//          .ToList();
//        throw new FileHasErrorsException(JsonConvert.SerializeObject(errors));
//      }

//      return new List<Error>();
//    }

//    private void SetClientNipAndClientNameAndClientId(ReadFile readFile)
//    {
//      foreach (var rahlClientData in _rahlQuery.GetRahlClientDataByCrmIds(readFile.ClientRows.Select(c => c.CrmId)))
//      {
//        foreach (var client in readFile.ClientRows.Where(cr => cr.CrmId.ToUpper() == rahlClientData.CrmId.ToUpper()))
//        {
//          client.NIP = rahlClientData.TaxId;
//          client.Name = rahlClientData.DisplayName;
//          client.ClientId = rahlClientData.ClientId;
//        }
//      }
//    }
//  }
//}