using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RW.VAC.Domain.API
{
    public interface IAutoAssemblyWorkClient
    {
        Task<(bool HasError, string Message, string Value1)> ReportWorkAsync( string lineCode , string lotSN , string stationCode , string workDate , string userCode , bool isProduction = true );

        Task<bool> QuerySNInfo( string sN , string ProductName );

        Task AirtightDataAPI( AirtightData airtightData );
        Task OilPressureDataAPI( OilPressureData airtightData );

        Task TorqueDataAPI(TorqueData torque );
    }
}
