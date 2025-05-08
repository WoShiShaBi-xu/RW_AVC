using RW.Framework.Domain.Services;
using RW.VAC.Domain.Products;

namespace RW.VAC.Domain.Records;

public class ProductionRecordManager(
    IProductionRecordRepository productionRecordRepository,
    IProductionDetailRepository productionDetailRepository) : DomainService
{
    public async Task CreateAsync(string serialNumber, string productName, string productCode, RoutingTypes routing,
        List<string> details, bool setCompleted = false)
    {
        var now = DateTime.Now;
        var record = new ProductionRecord(GuidGenerator.Greate(), serialNumber, productName, productCode, routing, now);
        await productionRecordRepository.InsertAsync(record);
        var detailList = new List<ProductionDetail>();
        foreach (var item in details.Select(detail =>
                     new ProductionDetail(GuidGenerator.Greate(), record.Id, detail, now)))
        {
            if (setCompleted) item.SetCompleted();
            detailList.Add(item);
        }

        await productionDetailRepository.InsertAsync(detailList);
    }
}