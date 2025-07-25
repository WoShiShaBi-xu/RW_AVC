using Microsoft.AspNetCore.Mvc;
using RW.VAC.Application.Contracts.AGV;
using RW.VAC.Application.Services.AGV;

namespace RW.VAC.WebApi.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class AgvController : ControllerBase
    {
        private readonly IAgvService _agvService;

        public AgvController( IAgvService agvService )
        {
            _agvService = agvService;
        }

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

       
        [HttpGet( "task/{taskId}/status" )]
        public async Task<IActionResult> GetTaskStatus( string taskId )
        {
            try
            {
                var result = await _agvService.GetTaskStatusAsync( taskId );
                return Ok( result );
            }
            catch (Exception ex)
            {
                return BadRequest( new { Success = false , Message = ex.Message } );
            }
        }
    }
}
