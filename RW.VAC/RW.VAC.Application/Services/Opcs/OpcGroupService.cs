using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Contracts.Orders;
using RW.VAC.Domain.Opcs;
using RW.VAC.Infrastructure.Opc;

namespace RW.VAC.Application.Services.Opcs;

//public class OpcGroupService( 

//    IOpcGroupRepository repository,
//	OpcGroupManager opcGroupManager,IUaClient uaClient, IProductionRecordService productionRecordService , IProductionDetailService productionDetailService, IProductionDataService productionDataService )
//	: CrudAppService<OpcGroup, Guid, OpcGroupDto, OpcGroupListRequestDto,
//		OpcGroupCreateUpdateDto, OpcGroupCreateUpdateDto>(repository), IOpcGroupService
//{
//    public override Task<OpcGroup> CreateAsync(OpcGroupCreateUpdateDto input)
//	{
//		return opcGroupManager.CreateAsync(input.Name!, input.Code!, input.Device!, input.Group);
//	}

//	public override Task<OpcGroup> UpdateAsync(Guid id, OpcGroupCreateUpdateDto input)
//	{
//		return opcGroupManager.UpdateAsync(id, input.Name!, input.Code!, input.Device!, input.Group);
//	}

//	public async Task DeleteWithItemAsync(Guid id)
//	{
//		using var uow = UnitOfWorkManager.Begin();
//		await opcGroupManager.DeleteAsync(id);
//		uow.Commit();
//	}
//	/// <summary>
//	/// 获取所有是工艺参数的点信息
//	/// </summary>
//	/// <param name="input"></param>
//	/// <returns></returns>
//	public async Task<List<OpcProcessDto>> GetOPCProcessListAsync()
//	{
//		var list = await Repository.Select.From<OpcItem>((o, p) => o.LeftJoin(op => op.Id == p.GroupId)).Where((o, p) =>p.IsProcess==true)
//			.ToListAsync((o, p) => new OpcProcessDto
//            {
//                GroupName = o.Name,
//                GroupDevice = o.Device,
//                GroupCode = o.Code,
//                ItemName = p.Name,
//                ItemDescription = p.Description
//            });
//		return list;
//	}

//    /// <summary>
//    /// 获取所有工艺参数点的信息并返回给前端
//    /// </summary>
//    public async Task<List<ProcessDto>> GetOPCProcessAsync()
//    {
	
//    }
	
//}
