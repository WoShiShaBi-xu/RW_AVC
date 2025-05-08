using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Contracts.Orders;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Services.Records;
using RW.VAC.Domain.Opcs;
using RW.VAC.Domain.Products;
using RW.VAC.Infrastructure.Opc;

namespace RW.VAC.Application.Services.Opcs;

public class OpcGroupService( 

    IOpcGroupRepository repository,
	OpcGroupManager opcGroupManager,IUaClient uaClient, IProductionRecordService productionRecordService , IProductionDetailService productionDetailService, IProductionDataService productionDataService )
	: CrudAppService<OpcGroup, Guid, OpcGroupDto, OpcGroupListRequestDto,
		OpcGroupCreateUpdateDto, OpcGroupCreateUpdateDto>(repository), IOpcGroupService
{
    public override Task<OpcGroup> CreateAsync(OpcGroupCreateUpdateDto input)
	{
		return opcGroupManager.CreateAsync(input.Name!, input.Code!, input.Device!, input.Group);
	}

	public override Task<OpcGroup> UpdateAsync(Guid id, OpcGroupCreateUpdateDto input)
	{
		return opcGroupManager.UpdateAsync(id, input.Name!, input.Code!, input.Device!, input.Group);
	}

	public async Task DeleteWithItemAsync(Guid id)
	{
		using var uow = UnitOfWorkManager.Begin();
		await opcGroupManager.DeleteAsync(id);
		uow.Commit();
	}
	/// <summary>
	/// 获取所有是工艺参数的点信息
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public async Task<List<OpcProcessDto>> GetOPCProcessListAsync()
	{
		var list = await Repository.Select.From<OpcItem>((o, p) => o.LeftJoin(op => op.Id == p.GroupId)).Where((o, p) =>p.IsProcess==true)
			.ToListAsync((o, p) => new OpcProcessDto
            {
                GroupName = o.Name,
                GroupDevice = o.Device,
                GroupCode = o.Code,
                ItemName = p.Name,
                ItemDescription = p.Description
            });
		return list;
	}

    /// <summary>
    /// 获取所有工艺参数点的信息并返回给前端
    /// </summary>
    public async Task<List<ProcessDto>> GetOPCProcessAsync()
    {
	
        List<OpcProcessDto> opcProcessList = await GetOPCProcessListAsync();

        var dataList = new List<ProcessDto>();
		ProcessDto process = new ProcessDto();	
        foreach (var item in opcProcessList)
        {
            var pointValue = uaClient.Read(item.GroupDevice + "." + item.ItemName);
			if (pointValue != null)
			{//赋值设定值，要去数据库获取SN+实时值
				if( item.GroupName =="液压试验")
				{
					process.HydraulicValue = pointValue.ToString();
                }
				if( item.GroupName == "气密性试验" )
				{
                    if ( item.ItemDescription == "上限设定值" )
                    {
                        process.AirtightValue = pointValue.ToString();
                    }
                    
                }
                if ( item.GroupName == "开关测试" )
                {
                    process.SwithValue = pointValue.ToString();
                }
            }
            //dataList.Add(new ProcessDto
            //{
            //      = item.GroupName,
            //    ItemDescription = item.ItemDescription,
            //    Value = pointValue.ToString()
            //});
        }
        var detail = await productionRecordService.GetTodayCompletedSerialNumbersAsync();
        foreach (var item in detail )//获取所有今天完成的二维码
		{//根据二维码获取质量数据
            var record = await productionRecordService.GetByAsync( item );//获取生产记录
            if ( record == null ) continue;

            var details1 = await productionDetailService.GetByAsync( record.Id, "液压试验" );//根据生产记录获取生产详情


            var details2 = await productionDetailService.GetByAsync( record.Id , "气密性试验" );//根据生产记录获取生产详情

            var details3 = await productionDetailService.GetByAsync( record.Id , "开关测试" );//根据生产记录获取生产详情
            string 液压泄露量 = "N/A";
            string 气密A测结果 = "N/A";
            string 气密B测结果 = "N/A";
            string 开关扭矩 = "N/A";
            if ( details1 != null )
            {
                var dataRequest = new ProdDataListRequestDto();
                dataRequest.DetailId = details1.Id;
                var data = await productionDataService.GetListAsync( dataRequest ); // 获取每个详情的工艺参数数据
                foreach ( var items in data )
                {
                    if ( items.Name == "泄露量" )
                    {
                        液压泄露量 = items.Value;
                    }
                }
            }
           
            if ( details3 != null && details2 != null )
            {
                var dataRequest1 = new ProdDataListRequestDto();
                dataRequest1.DetailId = details2.Id;
                var data1 = await productionDataService.GetListAsync( dataRequest1 ); // 获取每个详情的工艺参数数据
                foreach ( var items in data1 )
                {
                    if ( items.Name == "A测结果" )
                    {
                        气密A测结果 = items.Value;
                    }
                    if ( items.Name == "B测结果" )
                    {
                        气密B测结果 = items.Value;
                    }
                }
                var dataRequest2 = new ProdDataListRequestDto();
                dataRequest2.DetailId = details3.Id;
                var data2 = await productionDataService.GetListAsync( dataRequest2 ); // 获取每个详情的工艺参数数据
                foreach ( var items in data2 )
                {
                    if ( items.Name == "扭矩" )
                    {
                        开关扭矩 = items.Value;
                    }
                }
            }else

            {
                continue;
            }
            if ( double.TryParse( 开关扭矩 , out double number ) )
            {
                // 保留小数点后一位
                string result = number.ToString( "F1" );
                开关扭矩= result;
            }
            string Airtightpractice = "2";
            if ( 气密A测结果 =="0")
            {
                if( 气密B测结果 == "1" )
                {
                    Airtightpractice = "1";
                }
            }else if( 气密A测结果 == "1" && 气密B测结果 == "1" )
            {
                Airtightpractice = "1";
            }

            dataList.Add( new ProcessDto
            {
                AirtightValue = process.AirtightValue ,
                HydraulicValue = process.HydraulicValue ,
                SwithValue = process.SwithValue ,
                SN = item,
                HydraulicpracticeValue = 液压泄露量 ,
                AirtightpracticeValue = Airtightpractice ,
                SwithpracticeValue = 开关扭矩 ,
                
            
            } );

        }

        return dataList;
    }
	
}
