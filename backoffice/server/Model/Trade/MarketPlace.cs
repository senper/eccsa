using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Trade
{
    public class MarketPlace
    {
        public MarketPlace()
        {

        }
        static MarketPlace()
        {
            DefaultMarket = new MarketPlace()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                LocationId = Guid.Parse("00000000-0000-0000-0000-0000000f4240"), //Ethiopia
                NameEng = " ",
                NameLocal = " ",
                Latitude = 0,
                Longitude = 0

            };
        }
        public static MarketPlace DefaultMarket { get; }
        public Guid Id { get; set; }

        public string Code { get; set; }
        public String NameEng { get; set; }

        public String NameLocal { get; set; }

        public Guid LocationId { get; set; } = Guid.Parse("00000000-0000-0000-0000-0000000f4240");

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public virtual ICollection<CommodityPrice> CommodityPrices { get; set; }
    }
}