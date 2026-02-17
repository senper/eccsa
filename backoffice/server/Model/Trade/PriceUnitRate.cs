using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Trade
{
	public class PriceUnitRate : ICode
	{
		public Guid Id { get; set; }

		public string Code { get; set; }
		public String Name { get; set; }
		public String NameLocal { get; set; }


		public virtual ICollection<CommodityPrice> CommodityPrices { get; set; }


		
	}
}