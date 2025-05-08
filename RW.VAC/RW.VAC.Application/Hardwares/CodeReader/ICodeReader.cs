using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Hardwares.CodeReader;

public interface ICodeReader
{
    /// <summary>
    ///		记录生产数据
    /// </summary>
    /// <param name="serialNumber">产品序列号</param>
    /// <param name="processName">工序名称</param>
    /// <param name="product">产品信息</param>
    /// <returns></returns>
    Task MakeRecord(string serialNumber, string processName, Product product);
}