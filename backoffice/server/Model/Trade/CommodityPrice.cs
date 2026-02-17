using System;

namespace ECCSA.BackOffice.Server.Model.Trade
{

    public class CommodityPrice
    {
        public Guid Id { get; set; }
        public DateTime MarketDate { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid PriceUnitRateId { get; set; }

        public Guid MarketPlaceId { get; set; }

        public PriceTypeEnum PriceType { get; set; }


        public decimal PriceValue { get; set; }
        public virtual CommodityType CommodityType { get; set; }
        public virtual PriceUnitRate PriceUnitRate { get; set; }

        public virtual MarketPlace MarketPlace { get; set; }

    }
}