using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public enum Status
    {
        Default = 0,
        HTTP_Continue = 100,
        HTTP_SwitchingProtocols = 101,
        HTTP_OK = 200,
        HTTP_Created = 201,
        HTTP_Accepted = 202,
        HTTP_NonAuthoritativeInformation = 203,
        HTTP_NoContent = 204,
        HTTP_ResetContent = 205,
        HTTP_PartialContent = 206,
        HTTP_MultipleChoices = 300,
        HTTP_MovedPermanently = 301,
        HTTP_Found = 302,
        HTTP_SeeOther = 303,
        HTTP_NotModified = 304,
        HTTP_UseProxy = 305,
        HTTP_TemporaryRedirect = 307,
        HTTP_BadRequest = 400,
        HTTP_Unauthorized = 401,
        HTTP_PaymentRequired = 402,
        HTTP_Forbidden = 403,
        HTTP_NotFound = 404,
        HTTP_MethodNotAllowed = 405,
        HTTP_NotAcceptable = 406,
        HTTP_ProxyAuthenticationRequired = 407,
        HTTP_RequestTimeout = 408,
        HTTP_Conflict = 409,
        HTTP_Gone = 410,
        HTTP_LengthRequired = 411,
        HTTP_PreconditionFailed = 412,
        HTTP_RequestEntityTooLarge = 413,
        HTTP_RequestURITooLong = 414,
        HTTP_UnsupportedMediaType = 415,
        HTTP_RequestedRangeNotSatisfiable = 416,
        HTTP_ExpectationFailed = 417,
        HTTP_InternalServerError = 500,
        HTTP_NotImplemented = 501,
        HTTP_BadGateway = 502,
        HTTP_ServiceUnavailable = 503,
        HTTP_GatewayTimeout = 504,
        HTTP_HTTPVersionNotSupported = 505
    }

    public static class Extensions
    {
        public static ResultContract<T> SetResultState<T>(this ResultContract<T> result, Status status, T? outputRes)
        {
            result.Status = status;
            result.Message = string.Empty;
            result.IsSuccessful = status == Status.HTTP_OK;
            result.OutputRes = outputRes;
            return result;
        }
    }

    public class ResultContract<T>
    {
        public ResultContract()
        {
            this.SetResultState(Status.Default, default(T));
        }

        public Status Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public T? OutputRes { get; set; }
    }
}