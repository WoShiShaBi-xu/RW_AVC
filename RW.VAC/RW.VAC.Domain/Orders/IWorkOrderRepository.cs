using FreeSql;

namespace RW.VAC.Domain.Orders;

public interface IWorkOrderRepository : IBaseRepository<WorkOrder, Guid>
{
}