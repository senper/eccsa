using System;

namespace ECCSA.BackOffice.Server.Model.Trade
{
    public class ImportExport
    {
        public Guid Id { get; set; }
        public string HSCode { get; set; }
        public string CPCode { get; set; }

        public string CurrentEPC { get; set; }

        public string PreviousEPC { get; set; }

        public string APC { get; set; }
        public string Direction { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string OriginCountry { get; set; }

        public string DestinationCountry { get; set; }

        public string ConsignmentCountry { get; set; }

        public long NetWeightKg { get; set; }
        public decimal TradeValue { get; set; }

        public decimal TaxPaid { get; set; }

        public int NumberOfTrades { get; set; }

    }
}