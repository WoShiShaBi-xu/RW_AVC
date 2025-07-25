using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RW.VAC.Application.Contracts.AGV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Client.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route( "api/[controller]" )]
    public class AgvController : ControllerBase
    {
        private readonly IAgvService _agvService;

        public AgvController( IAgvService agvService )
        {
            _agvService = agvService;
        }

        /// <summary>
        /// AGV上料指令
        /// </summary>
        [HttpPost( "load" )]
        public async Task<IActionResult> SendLoadCommand( [FromBody] LoadCommandRequest request )
        {
            try
            {
                var result = await _agvService.SendLoadCommandAsync( "cst_FxU5B9ry" );
                return Ok( new { Success = result , Message = result ? "上料指令发送成功" : "上料指令发送失败" } );
            }
            catch (Exception ex)
            {
                return BadRequest( new { Success = false , Message = ex.Message } );
            }
        }
    }

    public class LoadCommandRequest
    {
        public string StationId { get; set; }
        public string PalletId { get; set; }
    }

    public class UnloadCommandRequest
    {
        public string StationId { get; set; }
        public string PalletId { get; set; }
    }

    public class TransportTaskRequest
    {
        public string SourceLocation { get; set; }
        public string TargetLocation { get; set; }
        public string PalletId { get; set; }
    }
}
