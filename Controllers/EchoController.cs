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
            var query = this.HttpContext.Request.Query;
            return query.ContainsKey(RESPONSE_STATUS) && int.TryParse(query[RESPONSE_STATUS], out var value) ? value : 200;
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpPut]
        [HttpPatch]
        public IActionResult Get()
        {

            _logger.LogInformation("Request: '{req}', Path: '{path}', Host: {host}, MachineName: {machineName}, headers: {headers}",
                this.HttpContext.TraceIdentifier, Request.Path, Request.Host, Environment.MachineName,
                Request.Headers.Select(a => $"{a.Key}: {a.Value}").Aggregate((e, a) => e + ", " + a));

            return new JsonResult(new EchoInfo()
            {
                Date = DateTime.Now,
                RequestData = new Dictionary<string, object> {
                   { "Headers",  Request.Headers},
                   { "Path", Request.Path},
                   { "Host", Request.Host },
                   { "EnvHost", Environment.MachineName},

                   //{ "Body", Request.Body},
               }
            })
            {
                StatusCode = GetResponseStatusCode()
            };
        }
    }
}