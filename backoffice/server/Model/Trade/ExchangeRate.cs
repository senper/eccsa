using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Trade
{
	public class ExchangeRate : ICode
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public DateTime TransactionDate { get; set; }
		public String Currency { get; set; }
		public decimal Buying { get; set; }
		public decimal Selling { get; set; }
		
	}

	
}