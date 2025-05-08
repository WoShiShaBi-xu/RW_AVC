using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Records
{
    public class ProductionDataBedstand
    {
        public static string _airtightblanking { get; set; }


        public static string _Oiltightblanking { get; set; }

    }
    public class ProductionBedstandDataresult
    {
        public string Aresult { get; set; }//A测结果
        public string AleakageRate { get; set; }//A测泄露量
        public string Bresult { get; set; }//B测结果
        public string BleakageRate { get; set; }//B测泄露量
        public string Testpressure { get; set; }//测试气压
        public string itemNumber { get; set; }//产品编号
    }
}
