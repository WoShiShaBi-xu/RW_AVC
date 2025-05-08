using RW.Framework.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Opcs
{
    public class OpcProcessDto : EntityDto<Guid>
    {
        public string GroupName { get; set; }

        public string GroupCode { get; set; }

        public string GroupDevice { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }
    }

    public class ProcessDto
    {
        public string SN { get; set; }
        public string HydraulicValue { get; set; }
        public string AirtightValue { get; set; }
        public string SwithValue { get; set; }

        public string HydraulicpracticeValue { get; set; }
        public string AirtightpracticeValue { get; set; }
        public string SwithpracticeValue { get; set; }

    }
}
