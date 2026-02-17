using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Context;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.API.Seeding
{
    public static partial class InitializeDatabase
    {
        private static void InitializeTrade(EccsaContext context)
        {
            InitializeExchangeRate(context);
            InitializeCommodities(context);
        }

        private static void InitializeExchangeRate(EccsaContext context)
        {
            
        }
        private static void InitializeCommodities(EccsaContext context)
        {
            var id1 = Sequence(1);
            var id2 = Sequence(2);
            var id3 = Sequence(3);
            var priceRates = new List<PriceUnitRate>
            {
                new PriceUnitRate
                {
                    Id = id1,
                    Name = "Birr/Quintal",
                    NameLocal="ብር በኩንታል"
                },
                new PriceUnitRate
                {
                    Id = id2,
                    Name = "Birr/Ferensula",
                    NameLocal="ብር በኩንታል"
                },
                new PriceUnitRate
                {
                    Id = id3,
                    Name = "Birr/Giram",
                    NameLocal="ብር በኩንታል"
                }
            };
            var lstTypes = new List<CommodityType>
            {
                new CommodityType
                {
                    Id = id1,
                    CommodityPrices = new List<CommodityPrice>
                    {
                        new CommodityPrice
                        {
                            Id = Sequence(101),
                            PriceValue = 44.7755M,
                            CommodityTypeId = id1,
                            PriceUnitRateId = id1,
                            MarketDate = DateTime.Today.AddDays(-2)
                        },
                        new CommodityPrice
                        {
                             Id = Sequence(102),
                            PriceValue = 41.7755M,
                            CommodityTypeId = id1,
                            PriceUnitRateId = id1,
                            MarketDate = DateTime.Today.AddDays(-3)
                        }
                    },
                    Name = "Coffee",
                    NameLocal = "ቡና "
                },
                new CommodityType
                {
                    Id = id2,
                    CommodityPrices = new List<CommodityPrice>
                    {
                        new CommodityPrice
                        {
                            Id = Sequence(201),
                            PriceValue = 144.7755M,

                            CommodityTypeId = id2,
                            PriceUnitRateId = id1,
                            MarketDate = DateTime.Today.AddDays(-2)
                        },
                        new CommodityPrice
                        {
                             Id = Sequence(202),
                            PriceValue = 101.7755M,
                            CommodityTypeId = id2,
                            PriceUnitRateId = id1,
                            MarketDate = DateTime.Today.AddDays(-3)
                        }
                    },
                    Name = "Coffee",
                    NameLocal = "ቡና "
                }
            };
            context.PriceUnitRates.AddRange(priceRates);
            context.CommodityTypes.AddRange(lstTypes);
        }
    }
}
