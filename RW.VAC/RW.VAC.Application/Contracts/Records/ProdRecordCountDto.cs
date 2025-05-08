using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Records;

public class ProdRecordCountDto
{
	public string Date { get; set; }
	public int Count { get; set; }
	public Dictionary<string,ProdTypeCount> Type { get; set; }
}

public class ProdTypeCount
{
	public int Count { get; set; }
	public string Type { get; set; }
}