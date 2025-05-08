using Autofac;
using Microsoft.AspNetCore.Mvc;
using RW.Framework.EventBus;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Infrastructure.Opc;
using RW.Framework.Extensions;
using RW.VAC.Dashboard.subscribe;
using RW.VAC.Application.Services.Opcs;
using RW.VAC.Dashboard.API.WebSocketManagers;


namespace RW.VAC.Dashboard.API.Controllers
{

    [ApiController]
    [Route( "[controller]" )]
    public class WeatherForecastController : ControllerBase
    {
        private readonly OpcNodeManager _nodeManager;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController( ILogger<WeatherForecastController> logger  , OpcNodeManager nodeManager)
        {
            _logger = logger;
            _nodeManager = nodeManager;
        }
        [HttpGet( "test" )]
        public IActionResult Test( )
        {
            return Ok( "API is working" );
        }

    }
}
