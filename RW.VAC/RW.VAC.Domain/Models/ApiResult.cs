using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Entities
{
	public class ApiResult
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public string SN { get; set; }
		public string MoCode { get; set; }
		public string ProductRootCode { get; set; }
		public string ProductName { get; set; }
	}
}
