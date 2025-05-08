using Autofac.Features.Indexed;
using RW.VAC.Domain.CodeReaders;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Hardwares.CodeReader;

public class CodeReaderAdaptor(
	IIndex<ProcessType, ICodeReader> handlers,
	IReadOnlyDictionary<string, (string process, ProcessType type)> cache)
{
	public async Task Do(string ip, string serialNumber, Product? product)
	{
		if(product == null) return;
		if (cache.TryGetValue(ip, out var info))
		{
			var handler = handlers[info.type];
			await handler.MakeRecord(serialNumber, info.process, product);
		}
	}
}