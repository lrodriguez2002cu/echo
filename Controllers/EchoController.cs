using Microsoft.AspNetCore.Mvc;

namespace echo.Controllers
{
    [ApiController]
    [Route("/{*path}")]

    public class EchoController : ControllerBase
    {

        const string RESPONSE_STATUS = "responseStatus";
        private readonly ILogger<EchoController> _logger;

        public EchoController(ILogger<EchoController> logger)
        {
            _logger = logger;
        }

        private int GetResponseStatusCode()
        {
            var query = Request.Query;
            if (query.ContainsKey(RESPONSE_STATUS) && int.TryParse(query[RESPONSE_STATUS], out var value))
            {
                return value;
            }
            else return 200;
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpPut]
        [HttpPatch]
        public IActionResult Get()
        {
            var resonseStatus = GetResponseStatusCode();

            _logger.LogInformation(
                "Request: '{req}',  " +
                "ResponseStatus: {responseStatus}, " +
                "Path: '{path}', " +
                "Host: {host}, MachineName: {machineName}, " +
                "headers: {headers}, " +
                "Query: {query}",
                this.HttpContext.TraceIdentifier, resonseStatus, Request.Path, Request.Host, Environment.MachineName,
                Request.Headers.Select(a => $"'{a.Key}: {a.Value}'").Aggregate("", (e, a) => e + ", " + a),
                Request.Query.Select(q => $"'{q.Key}: {q.Value}'").Aggregate("", (e, a) => e + ", " + a)
                );

            return new JsonResult(new EchoInfo()
            {
                Date = DateTime.Now,
                RequestData = new Dictionary<string, object> {
                   { "Headers",  Request.Headers.ToDictionary(h=> h.Key, h=> h.Value)},
                   { "Query",  Request.Query.ToDictionary(e => e.Key, v=> v.Value)},
                   { "Path", Request.Path},
                   { "Host", Request.Host },
                   { "EnvHost", Environment.MachineName},

                   //{ "Body", Request.Body},
               }
            })
            {
                StatusCode = resonseStatus
            };
        }
    }
}