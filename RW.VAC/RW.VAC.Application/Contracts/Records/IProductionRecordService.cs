using RW.Framework.Application.Services;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public interface IProductionRecordService : ICrudAppService<ProductionRecord, Guid, ProductionRecordDto,
	ProdRecordPagedListRequestDto, ProductionRecordCreateDto, ProductionRecord>
{
	Task<ProductionRecord?> GetByAsync(string serialNumber);

	Task CreateWithDetailAsync(ProductionRecordCreateDto dto, bool setCompleted = false);

	Task SetAbnormalOfflineAsync(string serialNumber);

	Task SetBackOnlineAsync(string serialNumber);

	Task SetCompletedAsync(string serialNumber);

	IList<ProdRecordCountDto> GetWeekCapacity();

	long GetDateCapacity(int type);

	long GetDateOffLine(int type);

	Task<List<string>> GetTodayCompletedSerialNumbersAsync();
}