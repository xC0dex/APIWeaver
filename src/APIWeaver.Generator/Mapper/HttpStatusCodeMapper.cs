using System.Net;

namespace APIWeaver;

internal static class HttpStatusCodeMapper
{
    internal static HttpStatusCode ToStatusCode(this string stringCode)
    {
        return stringCode switch
        {
            "200" => HttpStatusCode.OK,
            "201" => HttpStatusCode.Created,
            "202" => HttpStatusCode.Accepted,
            "204" => HttpStatusCode.NoContent,
            "301" => HttpStatusCode.MovedPermanently,
            "302" => HttpStatusCode.Found,
            "304" => HttpStatusCode.NotModified,
            "400" => HttpStatusCode.BadRequest,
            "401" => HttpStatusCode.Unauthorized,
            "403" => HttpStatusCode.Forbidden,
            "404" => HttpStatusCode.NotFound,
            "405" => HttpStatusCode.MethodNotAllowed,
            "409" => HttpStatusCode.Conflict,
            "410" => HttpStatusCode.Gone,
            "415" => HttpStatusCode.UnsupportedMediaType,
            "429" => HttpStatusCode.TooManyRequests,
            "500" => HttpStatusCode.InternalServerError,
            "502" => HttpStatusCode.BadGateway,
            "503" => HttpStatusCode.ServiceUnavailable,
            "504" => HttpStatusCode.GatewayTimeout,
            _ => throw new ArgumentOutOfRangeException(nameof(stringCode), stringCode, "Unknown status code")
        };
    }

    internal static string ToName(this string stringCode)
    {
        return stringCode switch
        {
            "200" => "Ok",
            "201" => "Created",
            "202" => "Accepted",
            "204" => "NoContent",
            "301" => "MovedPermanently",
            "302" => "Found",
            "304" => "NotModified",
            "400" => "BadRequest",
            "401" => "Unauthorized",
            "403" => "Forbidden",
            "404" => "NotFound",
            "405" => "MethodNotAllowed",
            "409" => "Conflict",
            "410" => "Gone",
            "415" => "UnsupportedMediaType",
            "429" => "TooManyRequests",
            "500" => "InternalServerError",
            "502" => "BadGateway",
            "503" => "ServiceUnavailable",
            "504" => "GatewayTimeout",
            _ => throw new ArgumentOutOfRangeException(nameof(stringCode), stringCode, "Unknown status code")
        };
    }
}