using Microsoft.AspNetCore.Mvc;

namespace echo.Controllers
{
    [ApiController]
    [Route("/{*path}")]

    public class EchoController : ControllerBase
    {

        private readonly ILogger<EchoController> _logger;

        public EchoController(ILogger<EchoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpPut]
        [HttpPatch]
        public IActionResult Get()
        {

            _logger.LogInformation("Request: '{req}', Path: '{path}', Host: {host}, MachineName: {machineName}", this.HttpContext.TraceIdentifier, Request.Path, Request.Host, Environment.MachineName);

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
            });
        }
    }
}