using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;
using TouchSocket.Core;

namespace RW.VAC.Application.Hardwares.Opc;

public class TrussControl(
    CodeQueue codeQueue,
   
    IGuidGenerator guidGenerator,
    ILogger<TrussControl> logger)
{
    public required TagStorage Tags { protected get; init; }
    /// <summary>
    ///     上料
    /// </summary>
    /// <param name="e"></param>
    public async void Feed(TagChangedEventArgs e)
    {
		
    }

    /// <summary>
    ///     下料
    /// </summary>
    /// <param name="e"></param>
    public async void Lower(TagChangedEventArgs e)
    {
      
    }
}