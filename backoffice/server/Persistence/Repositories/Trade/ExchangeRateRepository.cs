using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Trade;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Trade
{
    public class ExchangeRateRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public ExchangeRateRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<ExchangeRate> ExchangeRates(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.ExchangeRates;
		}

		public async Task<ExchangeRate> ExchangeRateById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.ExchangeRates.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<ExchangeRatePayload> Create(GenericInput<CreateExchangeRateInput> input)
		{
			ExchangeRate data = new()
			{
			Id = Guid.NewGuid()
			, TransactionDate = input.PayLoad.TransactionDate
			, Currency = input.PayLoad.Currency
			, Buying = input.PayLoad.Buying
			, Selling = input.PayLoad.Selling

			};
			await _context.ExchangeRates.AddAsync(data);
		   var val = await _context.SaveChangesAsync();
		   return new ExchangeRatePayload(data);
		}
		public async Task<List<ExchangeRate>> Save(List<ExchangeRate> data)
		{
			var transactionDate = data.Max(x => x.TransactionDate);
			var dataExists = await _context.ExchangeRates.AnyAsync(x => x.TransactionDate == transactionDate);
			if (!dataExists)
			{
				await _context.ExchangeRates.AddRangeAsync(data);
				await _context.SaveChangesAsync();
			}

			return data;
		}

		
		public async Task<ExchangeRatePayload> Update(GenericInput<UpdateExchangeRateInput> data)
		{
			var tmp = await  _context.ExchangeRates.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
									tmp.TransactionDate = data.PayLoad.TransactionDate;
			tmp.Currency = data.PayLoad.Currency;
			tmp.Buying = data.PayLoad.Buying;
			tmp.Selling = data.PayLoad.Selling;

			_context.ExchangeRates.Update(tmp);
			await _context.SaveChangesAsync();
			return new ExchangeRatePayload(tmp);

		}

		public async Task Delete(ExchangeRate data)
		{
			var tmp = await _context.ExchangeRates.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.ExchangeRates.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}