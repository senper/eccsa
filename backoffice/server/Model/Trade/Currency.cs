using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Trade
{
    public class Currency
	{
		public String Code { get; set; }
		public String OrderNo { get; set; }

		public String Name { get; set; }

		[NotMapped]
		public int ExchangeRatesCount { get; set; } = 0;
	}
}