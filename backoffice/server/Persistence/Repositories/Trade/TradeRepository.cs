using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Trade
{
    public class TradeRepository
    {
        private readonly EccsaContext _context;
        public TradeRepository(EccsaContext context)
        {
            _context = context;
        }

        public async Task Save(List<ExchangeRate> data)
        {
            //var transactionDate = data.Max(x => x.TransactionDate);

            foreach (var er in data)
            {
                var dataExists = await _context.ExchangeRates.AnyAsync(x => x.TransactionDate == er.TransactionDate && x.Currency == er.Currency);
                if (dataExists) continue;
                await _context.ExchangeRates.AddAsync(er);
            }
            await _context.SaveChangesAsync();

        }

        public async Task<List<ExchangeRate>> ExchangeRatesAll()
        {
            if (_context.ExchangeRates.Any())
            {
                var transactionDate = await _context.ExchangeRates.MaxAsync(x => x.TransactionDate);
                return await _context.ExchangeRates.Where(x => x.TransactionDate == transactionDate).ToListAsync();
            }
            else
            {
                return new List<ExchangeRate>();
            }

        }

        public async Task<List<ExchangeRate>> ExchangeRates()
        {
if (_context.ExchangeRates.Any())
            {
            var transactionDate = await _context.ExchangeRates.MaxAsync(x => x.TransactionDate);
            var sql = "SELECT trade.exchange_rates.* FROM 	trade.currencies 	INNER JOIN trade.exchange_rates ON trade.currencies.code = trade.exchange_rates.currency WHERE trade.exchange_rates.transaction_date = {0} ORDER BY 	trade.currencies.order_no ASC";
            var lst = await _context.ExchangeRates.FromSqlRaw(sql, transactionDate).Where(x => x.TransactionDate == transactionDate).ToListAsync();
            return lst; }
            else
            {
                return new List<ExchangeRate>();
            }

        }

        public async Task<List<CommodityType>> CommodityTypes()
        {
            return await _context.CommodityTypes.Include(x => x.CommodityPrices).ToListAsync();
        }
        public async Task<CommodityType> CommodityTypeById(Guid id)
        {
            return await _context.CommodityTypes.Include(x => x.CommodityPrices).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task DeleteCommodityType(Guid id)
        {
            var tmp = await _context.CommodityTypes.Include(x => x.CommodityPrices).FirstOrDefaultAsync(i => i.Id == id);
            if (tmp.CommodityPrices.Count > 0) return;
            _context.CommodityTypes.Remove(tmp);
            await _context.SaveChangesAsync();

        }
        public async Task Save(CommodityType data)
        {
            var tmp = await _context.CommodityTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null && data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                await _context.CommodityTypes.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            if (tmp != null)
            {
                tmp.Name = data.Name;
                tmp.NameLocal = data.NameLocal;
                _context.CommodityTypes.Update(tmp);
                await _context.SaveChangesAsync();
            }

        }
        public async Task<List<CommodityPrice>> CommodityPricesAll()
        {
            return await _context.CommodityPrices.Include(x => x.CommodityType).Include(x => x.PriceUnitRate).ToListAsync();
        }

        public async Task<List<CommodityPrice>> CommodityPrices()
        {
            if (_context.CommodityPrices.Any())
            {
                var tmp = await _context.CommodityPrices.GroupBy(g => g.CommodityTypeId).Select(x => new { TypeId = x.Key, LastDate = x.Max(y => y.MarketDate) }).ToListAsync();
                var lst = new List<CommodityPrice>();
                foreach (var key in tmp)
                {
                    var t = await _context.CommodityPrices.Include(x => x.CommodityType).Include(x => x.PriceUnitRate).Where(x => x.CommodityTypeId == key.TypeId && x.MarketDate == key.LastDate).ToListAsync();
                    lst.AddRange(t);
                }
                return lst;
            }
            else
            {
                return new List<CommodityPrice>();
            }
        }
        public async Task<CommodityPrice> CommodityPriceById(Guid id)
        {
            return await _context.CommodityPrices.Include(x => x.CommodityType).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task DeleteCommodityPrice(Guid id)
        {
            var tmp = await _context.CommodityPrices.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null) return;
            _context.CommodityPrices.Remove(tmp);
            await _context.SaveChangesAsync();

        }
        public async Task Save(CommodityPrice data)
        {
            var tmp = await _context.CommodityPrices.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null && data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                tmp.CommodityTypeId = data.CommodityTypeId;
                tmp.MarketDate = data.MarketDate.Date;
                tmp.PriceUnitRateId = data.PriceUnitRateId;
                tmp.PriceValue = data.PriceValue;
                await _context.CommodityPrices.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            if (tmp != null)
            {
                tmp.MarketDate = data.MarketDate.Date;
                tmp.PriceUnitRateId = data.PriceUnitRateId;
                tmp.PriceValue = data.PriceValue;
                _context.CommodityPrices.Update(tmp);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<PriceUnitRate>> PriceUnitRates()
        {
            return await _context.PriceUnitRates.Include(x => x.CommodityPrices).ToListAsync();
        }
        public async Task<PriceUnitRate> PriceUnitRateById(Guid id)
        {
            return await _context.PriceUnitRates.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task DeletePriceUnitRate(Guid id)
        {
            var tmp = await _context.PriceUnitRates.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp.CommodityPrices.Count > 0) return;
            _context.PriceUnitRates.Remove(tmp);
            await _context.SaveChangesAsync();

        }
        public async Task Save(PriceUnitRate data)
        {
            var tmp = await _context.PriceUnitRates.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null && data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();

                await _context.PriceUnitRates.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            if (tmp != null)
            {
                tmp.Name = data.Name;
                tmp.NameLocal = data.NameLocal;
                _context.PriceUnitRates.Update(tmp);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<Currency>> Currencies()
        {
            var lst = await _context.ExchangeCurrencies.ToListAsync();
            var exchanges = await _context.ExchangeRates.GroupBy(x => x.Currency).Select(x => new { Code = x.Key, No = x.Count() }).ToDictionaryAsync(x => x.Code);
            foreach (var c in lst)
            {
                if (exchanges.ContainsKey(c.Code))
                    c.ExchangeRatesCount = exchanges[c.Code].No;
                else
                    c.ExchangeRatesCount = 0;
            }

            return lst;
        }
        public async Task<Currency> CurrencyById(string code)
        {
            var exchanges = await _context.ExchangeRates.Where(x => x.Currency == code).GroupBy(x => x.Currency).Select(x => new { Code = x.Key, No = x.Count() }).ToDictionaryAsync(x => x.Code);
            var curr = await _context.ExchangeCurrencies.FirstOrDefaultAsync(i => i.Code == code);
            if (exchanges.ContainsKey(code))
                curr.ExchangeRatesCount = exchanges[code].No;
            else
                curr.ExchangeRatesCount = 0;
            return curr;
        }

        public async Task DeleteCurrency(string code)
        {
            var exchanges = await _context.ExchangeRates.Where(x => x.Currency == code).GroupBy(x => x.Currency).Select(x => new { Code = x.Key, No = x.Count() }).ToDictionaryAsync(x => x.Code);

            var tmp = await _context.ExchangeCurrencies.FirstOrDefaultAsync(i => i.Code == code);
            if (exchanges.ContainsKey(code)) return;
            _context.ExchangeCurrencies.Remove(tmp);
            await _context.SaveChangesAsync();

        }
        public async Task Save(Currency data)
        {
            var tmp = await _context.ExchangeCurrencies.FirstOrDefaultAsync(r => r.Code == data.Code);
            if (tmp == null)
            {

                await _context.ExchangeCurrencies.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            if (tmp != null)
            {
                _context.ExchangeCurrencies.Update(tmp);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<Subscription>> GetSubscriptions(Guid userId)
        {
            return await _context.Subscriptions.Where(s => s.UserId == userId && s.Status).ToListAsync();
        }
        public async Task ChangeSubscription(Guid userId, List<Subscription> subscriptions)
        {
            var userSubs = await _context.Subscriptions.Where(s => s.UserId == userId).ToListAsync();
            foreach (var subscription in subscriptions)
            {
                var userS = userSubs.FirstOrDefault(s => s.TopicId == subscription.TopicId);
                if (userS != null && userS.Status != subscription.Status)
                {                    
                    userS.Status = subscription.Status;
                    if (subscription.Status)
                    {
                        userS.SubscriptionDate = DateTime.Today;
                    }
                    else
                    {
                        userS.UnsubscriptionDate = DateTime.Today;
                    }
                    _context.Subscriptions.Update(userS);
                }
                else if(userS == null && subscription.Status)
                {
                    _context.Subscriptions.Add(new Subscription
                    {
                        TopicId = subscription.TopicId,
                        Id = Guid.NewGuid(),
                        Status = subscription.Status,
                        SubscriptionDate = DateTime.Today,
                        UserId = userId,
                    });
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}